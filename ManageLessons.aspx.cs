using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ManageLessons : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "teacher")
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadClasses();
                ResetForm();
            }
        }

        void LoadClasses()
        {
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT LevelID, LevelName FROM Levels WHERE Status='Active' ORDER BY LevelOrder, LevelName", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlClasses.DataSource = dt;
                ddlClasses.DataTextField = "LevelName";
                ddlClasses.DataValueField = "LevelID";
                ddlClasses.DataBind();
            }

            if (ddlClasses.Items.Count > 0)
                LoadLessons();
            else
            {
                gvLessons.DataSource = null;
                gvLessons.DataBind();
            }
        }

        void LoadLessons()
        {
            string query = @"
            SELECT
                L.LessonID,
                LV.LevelName,
                L.LessonTitle,
                L.Description,
                L.CreatedDate,
                CONVERT(VARCHAR(19), L.CreatedDate, 120) AS CreatedDateDisplay,

                ISNULL(MC.MaterialCount, 0) AS MaterialCount,

                CASE
                    WHEN ISNULL(MC.MaterialCount, 0) = 0 THEN 'No Materials'
                    WHEN QI.LastQuizDate IS NULL THEN 'No Quiz'
                    WHEN MI.LastMaterialDate > QI.LastQuizDate THEN 'Needs Regeneration'
                    ELSE 'Up to Date'
                END AS QuizStatus

            FROM Lessons L
            INNER JOIN Levels LV ON L.LevelID = LV.LevelID

            LEFT JOIN
            (
                SELECT
                    LM.LessonID,
                    COUNT(*) AS MaterialCount
                FROM LessonMaterials LM
                GROUP BY LM.LessonID
            ) MC ON L.LessonID = MC.LessonID

            LEFT JOIN
            (
                SELECT
                    LM.LessonID,
                    MAX(MAT.UploadDate) AS LastMaterialDate
                FROM LessonMaterials LM
                INNER JOIN LearningMaterials MAT ON LM.MaterialID = MAT.MaterialID
                GROUP BY LM.LessonID
            ) MI ON L.LessonID = MI.LessonID

            LEFT JOIN
            (
                SELECT
                    LessonID,
                    MAX(ISNULL(UpdatedDate, CreatedDate)) AS LastQuizDate
                FROM Quiz
                GROUP BY LessonID
            ) QI ON L.LessonID = QI.LessonID

            WHERE L.LevelID = @LevelID
            ORDER BY L.CreatedDate DESC";

            using (SqlDataAdapter da = new SqlDataAdapter(query, con))
            {
                da.SelectCommand.Parameters.AddWithValue("@LevelID", ddlClasses.SelectedValue);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvLessons.DataSource = dt;
                gvLessons.DataBind();
            }
        }

        protected void ddlClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLessons();
            ResetForm();
            HideMessage();
        }

        protected void btnAddLesson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLessonTitle.Text))
            {
                ShowMessage("Lesson title is required.", false);
                return;
            }

            SqlTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                int newLessonId;

                using (SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Lessons (LevelID, LessonTitle, Description, CreatedDate)
                      OUTPUT INSERTED.LessonID
                      VALUES (@LevelID, @LessonTitle, @Description, GETDATE())", con, trans))
                {
                    cmd.Parameters.AddWithValue("@LevelID", ddlClasses.SelectedValue);
                    cmd.Parameters.AddWithValue("@LessonTitle", txtLessonTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());

                    newLessonId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                SqlCommand cmdProgress = new SqlCommand(
                    @"INSERT INTO StudentProgress
                      (UserID, LessonID, Status, CompletionDate, QuizScore, QuizPassed, LastQuizAttemptDate)
                      SELECT
                          U.UserID,
                          @lessonId,
                          'Not Started',
                          NULL,
                          NULL,
                          0,
                          NULL
                      FROM Users U
                      INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                      INNER JOIN Roles R ON UR.RoleID = R.RoleID
                      LEFT JOIN StudentProgress SP
                          ON SP.UserID = U.UserID
                         AND SP.LessonID = @lessonId
                      WHERE R.RoleName = 'Student'
                        AND SP.ProgressID IS NULL", con, trans);

                cmdProgress.Parameters.AddWithValue("@lessonId", newLessonId);
                cmdProgress.ExecuteNonQuery();

                trans.Commit();

                ShowMessage("Lesson added successfully.", true);
                LoadLessons();
                ResetForm();
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null)
                        trans.Rollback();
                }
                catch
                {
                }

                ShowMessage("Error adding lesson: " + ex.Message, false);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        protected void btnUpdateLesson_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(hfLessonID.Value))
            {
                ShowMessage("No lesson selected for update.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLessonTitle.Text))
            {
                ShowMessage("Lesson title is required.", false);
                return;
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE Lessons
                      SET LevelID = @LevelID,
                          LessonTitle = @LessonTitle,
                          Description = @Description
                      WHERE LessonID = @LessonID", con))
                {
                    cmd.Parameters.AddWithValue("@LevelID", ddlClasses.SelectedValue);
                    cmd.Parameters.AddWithValue("@LessonTitle", txtLessonTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@LessonID", hfLessonID.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                ShowMessage("Lesson updated successfully.", true);
                LoadLessons();
                ResetForm();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                ShowMessage("Error updating lesson: " + ex.Message, false);
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ResetForm();
            HideMessage();
        }

        protected void gvLessons_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int lessonId;

            if (!int.TryParse(e.CommandArgument.ToString(), out lessonId))
                return;

            if (e.CommandName == "EditLesson")
            {
                LoadLessonForEdit(lessonId);
            }
            else if (e.CommandName == "DeleteLesson")
            {
                DeleteLesson(lessonId);
            }
            else if (e.CommandName == "ManageMaterials")
            {
                Response.Redirect("UploadMaterials.aspx?lessonId=" + lessonId);
            }
            else if (e.CommandName == "ManageQuiz")
            {
                Response.Redirect("Quiz.aspx?lessonId=" + lessonId);
            }
        }

        void LoadLessonForEdit(int lessonId)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT LessonID, LevelID, LessonTitle, Description
                      FROM Lessons
                      WHERE LessonID = @LessonID", con))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        hfLessonID.Value = dr["LessonID"].ToString();
                        ddlClasses.SelectedValue = dr["LevelID"].ToString();
                        txtLessonTitle.Text = dr["LessonTitle"].ToString();
                        txtDescription.Text = dr["Description"].ToString();

                        btnAddLesson.Visible = false;
                        btnUpdateLesson.Visible = true;
                        btnCancelEdit.Visible = true;

                        ShowMessage("Editing lesson ID " + lessonId + ".", true);
                    }

                    dr.Close();
                    con.Close();
                }

                LoadLessons();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                ShowMessage("Error loading lesson: " + ex.Message, false);
            }
        }

        void DeleteLesson(int lessonId)
        {
            try
            {
                con.Open();

                SqlCommand checkMaterials = new SqlCommand(
                    "SELECT COUNT(*) FROM LessonMaterials WHERE LessonID = @LessonID", con);
                checkMaterials.Parameters.AddWithValue("@LessonID", lessonId);

                int materialCount = Convert.ToInt32(checkMaterials.ExecuteScalar());

                SqlCommand checkQuiz = new SqlCommand(
                    "SELECT COUNT(*) FROM Quiz WHERE LessonID = @LessonID", con);
                checkQuiz.Parameters.AddWithValue("@LessonID", lessonId);

                int quizCount = Convert.ToInt32(checkQuiz.ExecuteScalar());

                if (materialCount > 0 || quizCount > 0)
                {
                    con.Close();
                    ShowMessage("Cannot delete this lesson because it already has linked materials or quiz records.", false);
                    return;
                }

                SqlCommand deleteProgress = new SqlCommand(
                    "DELETE FROM StudentProgress WHERE LessonID = @LessonID", con);
                deleteProgress.Parameters.AddWithValue("@LessonID", lessonId);
                deleteProgress.ExecuteNonQuery();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Lessons WHERE LessonID = @LessonID", con);
                cmd.Parameters.AddWithValue("@LessonID", lessonId);

                cmd.ExecuteNonQuery();
                con.Close();

                ShowMessage("Lesson deleted successfully.", true);
                LoadLessons();
                ResetForm();
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                    con.Close();

                ShowMessage("Error deleting lesson: " + ex.Message, false);
            }
        }

        void ResetForm()
        {
            hfLessonID.Value = "";
            txtLessonTitle.Text = "";
            txtDescription.Text = "";

            btnAddLesson.Visible = true;
            btnUpdateLesson.Visible = false;
            btnCancelEdit.Visible = false;
        }

        void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "msg success" : "msg error";
        }

        void HideMessage()
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
        }

        public string GetQuizStatusCss(string status)
        {
            switch (status)
            {
                case "No Materials":
                    return "statusNoMaterials";
                case "No Quiz":
                    return "statusNoQuiz";
                case "Needs Regeneration":
                    return "statusOutdated";
                case "Up to Date":
                    return "statusUpdated";
                default:
                    return "statusNoMaterials";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherPage.aspx");
        }
    }
}