using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class QuizReview : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;
            AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
            Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int lessonID;
                if (!int.TryParse(Request.QueryString["LessonID"], out lessonID))
                {
                    Response.Redirect("StudentPage.aspx");
                    return;
                }

                LoadReview(lessonID);
            }
        }

        void LoadReview(int lessonID)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            con.Open();

            SqlCommand cmdAttempt = new SqlCommand(
                @"SELECT TOP 1 QA.AttemptID, QA.Score, QA.TotalQuestions, QA.Status, QA.AttemptDate, Q.QuizTitle
                  FROM QuizAttempts QA
                  INNER JOIN Quiz Q ON QA.QuizID = Q.QuizID
                  WHERE Q.LessonID = @lessonID AND QA.UserID = @userID
                  ORDER BY QA.AttemptDate DESC",
                con);

            cmdAttempt.Parameters.AddWithValue("@lessonID", lessonID);
            cmdAttempt.Parameters.AddWithValue("@userID", userID);

            SqlDataReader drAttempt = cmdAttempt.ExecuteReader();

            if (!drAttempt.Read())
            {
                drAttempt.Close();
                con.Close();

                lblMessage.Text = "You have not attempted this quiz yet.";
                btnRetake.Visible = false;
                return;
            }

            int attemptID = Convert.ToInt32(drAttempt["AttemptID"]);
            int score = Convert.ToInt32(drAttempt["Score"]);
            int totalQuestions = Convert.ToInt32(drAttempt["TotalQuestions"]);
            string status = drAttempt["Status"].ToString();
            DateTime attemptDate = Convert.ToDateTime(drAttempt["AttemptDate"]);
            string quizTitle = drAttempt["QuizTitle"].ToString();

            lblQuizTitle.Text = quizTitle;
            lblResult.Text = "Score: " + score + " / " + totalQuestions + " - " + status;
            lblAttemptDate.Text = "Attempt Date: " + attemptDate.ToString("dd/MM/yyyy hh:mm tt");

            btnRetake.Visible = (status == "Fail");

            drAttempt.Close();

            SqlDataAdapter daReview = new SqlDataAdapter(
                @"SELECT QQ.QuestionText,
                         QAA.SelectedOption,
                         QQ.CorrectOption,
                         QQ.Explanation,
                         QAA.IsCorrect,
                         QQ.QuestionOrder
                  FROM QuizAttemptAnswers QAA
                  INNER JOIN QuizQuestions QQ ON QAA.QuestionID = QQ.QuestionID
                  WHERE QAA.AttemptID = @attemptID
                  ORDER BY QQ.QuestionOrder",
                con);

            daReview.SelectCommand.Parameters.AddWithValue("@attemptID", attemptID);

            DataTable dtReview = new DataTable();
            daReview.Fill(dtReview);

            rptReview.DataSource = dtReview;
            rptReview.DataBind();

            con.Close();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            int lessonID;
            if (int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("LessonMaterials.aspx?LessonID=" + lessonID);
            }
            else
            {
                Response.Redirect("StudentPage.aspx");
            }
        }

        protected void btnRetake_Click(object sender, EventArgs e)
        {
            int lessonID;
            if (int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("TakeQuiz.aspx?LessonID=" + lessonID + "&retake=1");
            }
        }
    }
}