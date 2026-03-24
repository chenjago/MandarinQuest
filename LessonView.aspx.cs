using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class LessonView : System.Web.UI.Page
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        private const string StudentQuizPage = "TakeQuiz.aspx";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "student")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int levelId = GetLevelIdFromQuery();

                if (levelId <= 0)
                {
                    ShowMessage("Invalid level. Please open lessons from a specific level.", false);
                    rptLessons.DataSource = null;
                    rptLessons.DataBind();
                    pnlEmpty.Visible = true;
                    return;
                }

                LoadLessons();
            }
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private int GetCurrentUserId()
        {
            int userId;
            int.TryParse(Convert.ToString(Session["UserID"]), out userId);
            return userId;
        }

        private int GetLevelIdFromQuery()
        {
            int levelId;
            return int.TryParse(Request.QueryString["levelId"], out levelId) ? levelId : 0;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "message success" : "message error";
        }

        private void LoadLessons()
        {
            int userId = GetCurrentUserId();
            int levelId = GetLevelIdFromQuery();

            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = @"
                SELECT
                    L.LessonID,
                    L.LessonTitle,
                    L.Description,
                    LV.LevelName,
                    ISNULL(SP.Status, 'Not Started') AS StudentStatus,
                    ISNULL(SP.QuizPassed, 0) AS QuizPassed,
                    ISNULL(SP.QuizScore, 0) AS QuizScore,
                    SP.LastQuizAttemptDate,
                    CASE WHEN Q.QuizID IS NULL THEN 0 ELSE 1 END AS HasQuiz
                FROM Lessons L
                INNER JOIN Levels LV ON L.LevelID = LV.LevelID
                LEFT JOIN
                (
                    SELECT QuizID, LessonID
                    FROM Quiz
                    WHERE QuizStatus = 'Published'
                ) Q ON Q.LessonID = L.LessonID
                LEFT JOIN StudentProgress SP
                    ON SP.UserID = @UserID
                   AND SP.LessonID = L.LessonID
                WHERE LV.Status = 'Active'
                  AND L.LevelID = @LevelID
                ORDER BY L.LessonTitle ASC";

                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@LevelID", levelId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                rptLessons.DataSource = dt;
                rptLessons.DataBind();

                pnlEmpty.Visible = dt.Rows.Count == 0;
            }
        }

        protected void rptLessons_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            Label lblLessonStatus = (Label)e.Item.FindControl("lblLessonStatus");
            Label lblQuizInfo = (Label)e.Item.FindControl("lblQuizInfo");
            Button btnAction = (Button)e.Item.FindControl("btnAction");

            string studentStatus = ((HiddenField)e.Item.FindControl("hfStudentStatus")).Value;
            bool hasQuiz = ((HiddenField)e.Item.FindControl("hfHasQuiz")).Value == "1";
            bool quizPassed = ((HiddenField)e.Item.FindControl("hfQuizPassed")).Value == "True" ||
                              ((HiddenField)e.Item.FindControl("hfQuizPassed")).Value == "1";

            int quizScore = 0;
            int.TryParse(((HiddenField)e.Item.FindControl("hfQuizScore")).Value, out quizScore);

            string lastAttemptRaw = ((HiddenField)e.Item.FindControl("hfLastQuizAttemptDate")).Value;
            bool hasAttemptedQuiz = !string.IsNullOrWhiteSpace(lastAttemptRaw);

            if (!hasQuiz)
            {
                bool alreadyCompleted = string.Equals(studentStatus, "Completed", StringComparison.OrdinalIgnoreCase);

                if (alreadyCompleted)
                {
                    lblLessonStatus.Text = "Completed";
                    lblLessonStatus.CssClass = "badge status-completed";

                    lblQuizInfo.Text = "This lesson has no quiz and has already been completed.";

                    btnAction.Text = "Completed";
                    btnAction.Enabled = false;
                    btnAction.CssClass = "btn btn-completed";
                    btnAction.CommandName = "completed";
                }
                else
                {
                    lblLessonStatus.Text = "No Quiz";
                    lblLessonStatus.CssClass = "badge status-locked";

                    lblQuizInfo.Text = "No published quiz is available for this lesson. You can complete the lesson manually.";

                    btnAction.Text = "Complete Lesson";
                    btnAction.Enabled = true;
                    btnAction.CssClass = "btn btn-green";
                    btnAction.CommandName = "manualcomplete";
                }

                return;
            }

            if (quizPassed || quizScore >= 4 || string.Equals(studentStatus, "Completed", StringComparison.OrdinalIgnoreCase))
            {
                lblLessonStatus.Text = "Completed";
                lblLessonStatus.CssClass = "badge status-completed";

                lblQuizInfo.Text = "Quiz passed with " + quizScore + "/5.";

                btnAction.Text = "Completed";
                btnAction.Enabled = false;
                btnAction.CssClass = "btn btn-completed";
                btnAction.CommandName = "completed";
                return;
            }

            if (!hasAttemptedQuiz)
            {
                lblLessonStatus.Text = "Take Quiz";
                lblLessonStatus.CssClass = "badge status-takequiz";

                lblQuizInfo.Text = "You have not taken the quiz yet. You need at least 4/5 to pass.";

                btnAction.Text = "Take Quiz";
                btnAction.Enabled = true;
                btnAction.CssClass = "btn btn-orange";
                btnAction.CommandName = "quiz";
                return;
            }

            lblLessonStatus.Text = "Retake Quiz";
            lblLessonStatus.CssClass = "badge status-retake";

            lblQuizInfo.Text = "Latest score: " + quizScore + "/5. You need 4/5 to pass.";

            btnAction.Text = "Retake Quiz";
            btnAction.Enabled = true;
            btnAction.CssClass = "btn btn-red";
            btnAction.CommandName = "quiz";
        }

        protected void OpenLesson(object sender, CommandEventArgs e)
        {
            int lessonId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out lessonId))
                return;

            Response.Redirect("LessonMaterials.aspx?lessonId=" + lessonId);
        }

        protected void ActionLesson(object sender, CommandEventArgs e)
        {
            int lessonId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out lessonId))
                return;

            string commandName = Convert.ToString(e.CommandName);

            if (commandName == "quiz")
            {
                Response.Redirect(StudentQuizPage + "?LessonID=" + lessonId);
                return;
            }

            if (commandName == "manualcomplete")
            {
                CompleteLessonWithoutQuiz(lessonId);
                LoadLessons();
                return;
            }
        }

        private void CompleteLessonWithoutQuiz(int lessonId)
        {
            int userId = GetCurrentUserId();

            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE StudentProgress
                  SET Status = 'Completed',
                      CompletionDate = GETDATE()
                  WHERE UserID = @UserID AND LessonID = @LessonID", con))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@LessonID", lessonId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            ShowMessage("Lesson completed successfully.", true);
            LoadLessons();
        }
    }
}