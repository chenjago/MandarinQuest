using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using MandarinQuest.Models;
using MandarinQuest.Services;

namespace MandarinQuest
{
    public partial class Quiz : System.Web.UI.Page
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "teacher")
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                if (Request.QueryString["lessonId"] == null)
                {
                    ShowMessage("Lesson ID is missing.", false);
                    DisableButtons();
                    return;
                }

                int lessonId;
                if (!int.TryParse(Request.QueryString["lessonId"], out lessonId))
                {
                    ShowMessage("Invalid lesson ID.", false);
                    DisableButtons();
                    return;
                }

                LoadLessonInfo(lessonId);
                LoadQuizInfo(lessonId);
                LoadQuizQuestions(lessonId);
            }
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        void DisableButtons()
        {
            btnGenerateDummyQuiz.Enabled = false;
            btnRegenerateQuiz.Enabled = false;
            btnApproveQuiz.Enabled = false;
            btnDeleteQuiz.Enabled = false;
        }

        int GetLessonId()
        {
            int lessonId = 0;
            int.TryParse(Request.QueryString["lessonId"], out lessonId);
            return lessonId;
        }

        void LoadLessonInfo(int lessonId)
        {
            try
            {
                using (SqlConnection con = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT LessonID, LessonTitle
                      FROM Lessons
                      WHERE LessonID = @LessonID", con))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblLessonID.Text = dr["LessonID"].ToString();
                            lblLessonTitle.Text = dr["LessonTitle"].ToString();
                        }
                        else
                        {
                            lblLessonID.Text = lessonId.ToString();
                            lblLessonTitle.Text = "Lesson not found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading lesson info: " + ex.Message, false);
            }
        }

        void LoadQuizInfo(int lessonId)
        {
            try
            {
                using (SqlConnection con = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 1 QuizID, QuizTitle, QuizStatus, CreatedDate, UpdatedDate
              FROM Quiz
              WHERE LessonID = @LessonID
              ORDER BY QuizID DESC", con))
                {
                    cmd.Parameters.AddWithValue("@LessonID", lessonId);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            lblQuizTitle.Text = dr["QuizTitle"].ToString();

                            string status = dr["QuizStatus"].ToString();
                            lblQuizStatus.Text = "<span class='statusBadge " + GetStatusCss(status) + "'>" + status + "</span>";

                            btnRegenerateQuiz.Visible = true;
                            btnApproveQuiz.Visible = status == "Draft";
                            btnDeleteQuiz.Visible = true;
                        }
                        else
                        {
                            lblQuizTitle.Text = "No quiz available";
                            lblQuizStatus.Text = "<span class='statusBadge statusNoQuiz'>No Quiz</span>";

                            btnRegenerateQuiz.Visible = false;
                            btnApproveQuiz.Visible = false;
                            btnDeleteQuiz.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading quiz info: " + ex.Message, false);
            }
        }

        void LoadQuizQuestions(int lessonId)
        {
            try
            {
                using (SqlConnection con = CreateConnection())
                using (SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT qq.QuestionID,
                             qq.QuestionText,
                             qq.CorrectOption,
                             qq.Explanation,
                             qq.QuestionOrder
                      FROM Quiz q
                      INNER JOIN QuizQuestions qq ON q.QuizID = qq.QuizID
                      WHERE q.LessonID = @LessonID
                        AND q.QuizID = (
                            SELECT TOP 1 QuizID
                            FROM Quiz
                            WHERE LessonID = @LessonID
                            ORDER BY QuizID DESC
                        )
                      ORDER BY qq.QuestionOrder", con))
                {
                    da.SelectCommand.Parameters.AddWithValue("@LessonID", lessonId);

                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptQuestions.DataSource = dt;
                        rptQuestions.DataBind();
                        pnlNoQuiz.Visible = false;
                    }
                    else
                    {
                        rptQuestions.DataSource = null;
                        rptQuestions.DataBind();
                        pnlNoQuiz.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Error loading quiz questions: " + ex.Message, false);
            }
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hfQuestionID = (HiddenField)e.Item.FindControl("hfQuestionID");
                Repeater rptOptions = (Repeater)e.Item.FindControl("rptOptions");

                int questionId;
                if (hfQuestionID != null && int.TryParse(hfQuestionID.Value, out questionId))
                {
                    using (SqlConnection con = CreateConnection())
                    using (SqlDataAdapter da = new SqlDataAdapter(
                        @"SELECT OptionLabel, OptionText
                          FROM QuizOptions
                          WHERE QuestionID = @QuestionID
                          ORDER BY OptionLabel", con))
                    {
                        da.SelectCommand.Parameters.AddWithValue("@QuestionID", questionId);

                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        rptOptions.DataSource = dt;
                        rptOptions.DataBind();
                    }
                }
            }
        }

        protected void btnGenerateDummyQuiz_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                await GenerateQuizWorkflowAsync(false);
            }));
        }

        protected void btnRegenerateQuiz_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                await GenerateQuizWorkflowAsync(true);
            }));
        }

        private async Task GenerateQuizWorkflowAsync(bool isRegenerate)
        {
            int lessonId = GetLessonId();

            if (lessonId == 0)
            {
                ShowMessage("Invalid lesson ID.", false);
                return;
            }

            try
            {
                if (!isRegenerate)
                {
                    bool quizExists = QuizExists(lessonId);
                    if (quizExists)
                    {
                        ShowMessage("Quiz already exists. Use Regenerate Quiz instead.", false);
                        LoadQuizInfo(lessonId);
                        LoadQuizQuestions(lessonId);
                        return;
                    }
                }
                else
                {
                    DeleteQuizByLesson(lessonId);
                }

                await GenerateAiQuizAsync(lessonId);

                ShowMessage(isRegenerate ? "AI quiz regenerated successfully." : "AI quiz generated successfully.", true);
                LoadQuizInfo(lessonId);
                LoadQuizQuestions(lessonId);
            }
            catch (Exception ex)
            {
                ShowMessage((isRegenerate ? "Error regenerating AI quiz: " : "Error generating AI quiz: ") + ex.Message, false);
            }
        }

        private bool QuizExists(int lessonId)
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM Quiz WHERE LessonID = @LessonID", con))
            {
                checkCmd.Parameters.AddWithValue("@LessonID", lessonId);
                con.Open();
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                return count > 0;
            }
        }

        async Task GenerateAiQuizAsync(int lessonId)
        {
            List<string> filePaths = GetLessonMaterialPaths(lessonId);

            if (filePaths.Count == 0)
                throw new Exception("No lesson materials found for this lesson.");

            MaterialTextExtractor extractor = new MaterialTextExtractor();
            string materialText = extractor.ExtractCombinedText(filePaths);

            if (string.IsNullOrWhiteSpace(materialText))
                throw new Exception("Could not extract text from lesson materials.");

            OpenAIQuizService aiService = new OpenAIQuizService();
            QuizAiResponse aiQuiz = await aiService.GenerateQuizAsync(lblLessonTitle.Text, materialText, 5);

            if (aiQuiz == null || aiQuiz.Questions == null || aiQuiz.Questions.Count == 0)
                throw new Exception("AI returned an empty quiz.");

            SaveAiQuizToDatabase(lessonId, aiQuiz);
        }

        List<string> GetLessonMaterialPaths(int lessonId)
        {
            List<string> filePaths = new List<string>();

            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT LM.FilePath
                  FROM LessonMaterials LSM
                  INNER JOIN LearningMaterials LM ON LSM.MaterialID = LM.MaterialID
                  WHERE LSM.LessonID = @LessonID
                  ORDER BY LM.UploadDate DESC", con))
            {
                cmd.Parameters.AddWithValue("@LessonID", lessonId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        string dbPath = Convert.ToString(dr["FilePath"]);

                        if (string.IsNullOrWhiteSpace(dbPath))
                            continue;

                        string physicalPath = dbPath;

                        if (!Path.IsPathRooted(physicalPath))
                        {
                            if (physicalPath.StartsWith("~/"))
                                physicalPath = Server.MapPath(physicalPath);
                            else
                                physicalPath = Server.MapPath("~/" + physicalPath.TrimStart('/'));
                        }

                        if (File.Exists(physicalPath))
                            filePaths.Add(physicalPath);
                    }
                }
            }

            return filePaths;
        }

        void SaveAiQuizToDatabase(int lessonId, QuizAiResponse aiQuiz)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    SqlCommand quizCmd = new SqlCommand(
                        @"INSERT INTO Quiz (LessonID, QuizTitle, GeneratedByAI, QuizStatus, CreatedDate, UpdatedDate)
                          OUTPUT INSERTED.QuizID
                          VALUES (@LessonID, @QuizTitle, 1, 'Draft', GETDATE(), GETDATE())", con, tran);

                    quizCmd.Parameters.AddWithValue("@LessonID", lessonId);
                    quizCmd.Parameters.AddWithValue("@QuizTitle", aiQuiz.QuizTitle);

                    int quizId = Convert.ToInt32(quizCmd.ExecuteScalar());

                    int questionOrder = 1;

                    foreach (QuizAiQuestion q in aiQuiz.Questions)
                    {
                        SqlCommand qCmd = new SqlCommand(
                            @"INSERT INTO QuizQuestions (QuizID, QuestionText, CorrectOption, Explanation, QuestionOrder)
                              OUTPUT INSERTED.QuestionID
                              VALUES (@QuizID, @QuestionText, @CorrectOption, @Explanation, @QuestionOrder)", con, tran);

                        qCmd.Parameters.AddWithValue("@QuizID", quizId);
                        qCmd.Parameters.AddWithValue("@QuestionText", q.QuestionText);
                        qCmd.Parameters.AddWithValue("@CorrectOption", q.CorrectOption);
                        qCmd.Parameters.AddWithValue("@Explanation", q.Explanation ?? "");
                        qCmd.Parameters.AddWithValue("@QuestionOrder", questionOrder);

                        int questionId = Convert.ToInt32(qCmd.ExecuteScalar());

                        foreach (QuizAiOption option in q.Options.OrderBy(x => x.Label))
                        {
                            SqlCommand oCmd = new SqlCommand(
                                @"INSERT INTO QuizOptions (QuestionID, OptionLabel, OptionText)
                                  VALUES (@QuestionID, @OptionLabel, @OptionText)", con, tran);

                            oCmd.Parameters.AddWithValue("@QuestionID", questionId);
                            oCmd.Parameters.AddWithValue("@OptionLabel", option.Label);
                            oCmd.Parameters.AddWithValue("@OptionText", option.Text);

                            oCmd.ExecuteNonQuery();
                        }

                        questionOrder++;
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        void DeleteQuizByLesson(int lessonId)
        {
            using (SqlConnection con = CreateConnection())
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    List<int> quizIds = new List<int>();

                    using (SqlCommand getQuizCmd = new SqlCommand(
                        @"SELECT QuizID
                          FROM Quiz
                          WHERE LessonID = @LessonID", con, tran))
                    {
                        getQuizCmd.Parameters.AddWithValue("@LessonID", lessonId);

                        using (SqlDataReader dr = getQuizCmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                quizIds.Add(Convert.ToInt32(dr["QuizID"]));
                            }
                        }
                    }

                    foreach (int quizId in quizIds)
                    {
                        SqlCommand deleteOptions = new SqlCommand(
                            @"DELETE FROM QuizOptions
                              WHERE QuestionID IN (
                                  SELECT QuestionID FROM QuizQuestions WHERE QuizID = @QuizID
                              )", con, tran);
                        deleteOptions.Parameters.AddWithValue("@QuizID", quizId);
                        deleteOptions.ExecuteNonQuery();

                        SqlCommand deleteQuestions = new SqlCommand(
                            "DELETE FROM QuizQuestions WHERE QuizID = @QuizID", con, tran);
                        deleteQuestions.Parameters.AddWithValue("@QuizID", quizId);
                        deleteQuestions.ExecuteNonQuery();

                        SqlCommand deleteQuiz = new SqlCommand(
                            "DELETE FROM Quiz WHERE QuizID = @QuizID", con, tran);
                        deleteQuiz.Parameters.AddWithValue("@QuizID", quizId);
                        deleteQuiz.ExecuteNonQuery();
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
        }

        string GetStatusCss(string status)
        {
            switch (status)
            {
                case "Draft":
                    return "statusDraft";
                case "Published":
                case "Ready":
                    return "statusReady";
                default:
                    return "statusNoQuiz";
            }
        }

        int UpdateLatestQuizStatus(int lessonId, string newStatus)
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Quiz
          SET QuizStatus = @QuizStatus,
              UpdatedDate = GETDATE()
          WHERE QuizID = (
              SELECT TOP 1 QuizID
              FROM Quiz
              WHERE LessonID = @LessonID
              ORDER BY QuizID DESC
          )", con))
            {
                cmd.Parameters.AddWithValue("@LessonID", lessonId);
                cmd.Parameters.AddWithValue("@QuizStatus", newStatus);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "message success" : "message error";
        }

        protected void btnDeleteQuiz_Click(object sender, EventArgs e)
        {
            int lessonId = GetLessonId();

            if (lessonId == 0)
            {
                ShowMessage("Invalid lesson ID.", false);
                return;
            }

            try
            {
                if (!QuizExists(lessonId))
                {
                    ShowMessage("No quiz found to delete.", false);
                    return;
                }

                DeleteQuizByLesson(lessonId);

                lblQuizTitle.Text = "No quiz available";
                lblQuizStatus.Text = "<span class='statusBadge statusNoQuiz'>No Quiz</span>";

                rptQuestions.DataSource = null;
                rptQuestions.DataBind();
                pnlNoQuiz.Visible = true;

                btnRegenerateQuiz.Visible = false;
                btnApproveQuiz.Visible = false;
                btnDeleteQuiz.Visible = false;

                ShowMessage("Quiz deleted successfully.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting quiz: " + ex.Message, false);
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            int lessonId = GetLessonId();
            LoadLessonInfo(lessonId);
            LoadQuizInfo(lessonId);
            LoadQuizQuestions(lessonId);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageLessons.aspx");
        }

        protected void btnApproveQuiz_Click(object sender, EventArgs e)
        {
            int lessonId = GetLessonId();

            if (lessonId == 0)
            {
                ShowMessage("Invalid lesson ID.", false);
                return;
            }

            try
            {
                int rows = UpdateLatestQuizStatus(lessonId, "Published");

                if (rows > 0)
                {
                    ShowMessage("Quiz approved successfully.", true);
                }
                else
                {
                    ShowMessage("No quiz found to approve.", false);
                }

                LoadLessonInfo(lessonId);
                LoadQuizInfo(lessonId);
                LoadQuizQuestions(lessonId);
            }
            catch (Exception ex)
            {
                ShowMessage("Error approving quiz: " + ex.Message, false);
            }
        }
    }
}