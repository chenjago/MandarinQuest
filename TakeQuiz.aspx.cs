using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class TakeQuiz : System.Web.UI.Page
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

                LoadQuiz(lessonID);
            }
        }

        void LoadQuiz(int lessonID)
        {
            SqlCommand cmdQuiz = new SqlCommand(
                @"SELECT TOP 1 QuizID, QuizTitle
                  FROM Quiz
                  WHERE LessonID = @lessonID AND QuizStatus = 'Published'",
                con);

            cmdQuiz.Parameters.AddWithValue("@lessonID", lessonID);

            con.Open();

            object quizIDObj = null;
            string quizTitle = "";

            SqlDataReader drQuiz = cmdQuiz.ExecuteReader();
            if (drQuiz.Read())
            {
                quizIDObj = drQuiz["QuizID"];
                quizTitle = drQuiz["QuizTitle"].ToString();
            }
            drQuiz.Close();

            if (quizIDObj == null)
            {
                con.Close();
                lblMessage.Text = "No published quiz found for this lesson.";
                btnSubmit.Visible = false;
                return;
            }

            int quizID = Convert.ToInt32(quizIDObj);
            ViewState["QuizID"] = quizID;
            lblQuizTitle.Text = quizTitle;

            SqlDataAdapter daQuestions = new SqlDataAdapter(
                @"SELECT QuestionID, QuestionText, QuestionOrder
                  FROM QuizQuestions
                  WHERE QuizID = @quizID
                  ORDER BY QuestionOrder",
                con);

            daQuestions.SelectCommand.Parameters.AddWithValue("@quizID", quizID);

            DataTable dtQuestions = new DataTable();
            daQuestions.Fill(dtQuestions);

            rptQuestions.DataSource = dtQuestions;
            rptQuestions.DataBind();

            con.Close();
        }

        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = (DataRowView)e.Item.DataItem;
                int questionID = Convert.ToInt32(row["QuestionID"]);

                RadioButtonList rblOptions = (RadioButtonList)e.Item.FindControl("rblOptions");

                SqlDataAdapter daOptions = new SqlDataAdapter(
                    @"SELECT OptionLabel, OptionText
                      FROM QuizOptions
                      WHERE QuestionID = @questionID
                      ORDER BY OptionLabel",
                    con);

                daOptions.SelectCommand.Parameters.AddWithValue("@questionID", questionID);

                DataTable dtOptions = new DataTable();
                daOptions.Fill(dtOptions);

                rblOptions.DataSource = dtOptions;
                rblOptions.DataTextField = "OptionText";
                rblOptions.DataValueField = "OptionLabel";
                rblOptions.DataBind();

                for (int i = 0; i < rblOptions.Items.Count; i++)
                {
                    string label = dtOptions.Rows[i]["OptionLabel"].ToString();
                    rblOptions.Items[i].Text = label + ". " + rblOptions.Items[i].Text;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ViewState["QuizID"] == null)
            {
                lblMessage.Text = "Quiz not found.";
                return;
            }

            int lessonID;
            if (!int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("StudentPage.aspx");
                return;
            }

            int quizID = Convert.ToInt32(ViewState["QuizID"]);
            int userID = Convert.ToInt32(Session["UserID"]);
            int totalQuestions = rptQuestions.Items.Count;

            if (totalQuestions == 0)
            {
                lblMessage.Text = "No questions found.";
                return;
            }

            List<int> questionIDs = new List<int>();
            List<string> selectedOptions = new List<string>();

            foreach (RepeaterItem item in rptQuestions.Items)
            {
                HiddenField hfQuestionID = (HiddenField)item.FindControl("hfQuestionID");
                RadioButtonList rblOptions = (RadioButtonList)item.FindControl("rblOptions");

                if (string.IsNullOrEmpty(rblOptions.SelectedValue))
                {
                    lblMessage.Text = "Please answer all questions before submitting.";
                    return;
                }

                questionIDs.Add(Convert.ToInt32(hfQuestionID.Value));
                selectedOptions.Add(rblOptions.SelectedValue);
            }

            int score = 0;
            List<bool> correctFlags = new List<bool>();

            SqlTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                for (int i = 0; i < questionIDs.Count; i++)
                {
                    SqlCommand cmdCorrect = new SqlCommand(
                        @"SELECT CorrectOption
                          FROM QuizQuestions
                          WHERE QuestionID = @questionID",
                        con, trans);

                    cmdCorrect.Parameters.AddWithValue("@questionID", questionIDs[i]);

                    string correctOption = Convert.ToString(cmdCorrect.ExecuteScalar());
                    bool isCorrect = string.Equals(selectedOptions[i], correctOption, StringComparison.OrdinalIgnoreCase);

                    correctFlags.Add(isCorrect);

                    if (isCorrect)
                    {
                        score++;
                    }
                }

                string status = (score >= 4) ? "Pass" : "Fail";

                SqlCommand cmdAttempt = new SqlCommand(
                    @"INSERT INTO QuizAttempts (QuizID, UserID, Score, TotalQuestions, Status)
                      OUTPUT INSERTED.AttemptID
                      VALUES (@quizID, @userID, @score, @totalQuestions, @status)",
                    con, trans);

                cmdAttempt.Parameters.AddWithValue("@quizID", quizID);
                cmdAttempt.Parameters.AddWithValue("@userID", userID);
                cmdAttempt.Parameters.AddWithValue("@score", score);
                cmdAttempt.Parameters.AddWithValue("@totalQuestions", totalQuestions);
                cmdAttempt.Parameters.AddWithValue("@status", status);

                int attemptID = Convert.ToInt32(cmdAttempt.ExecuteScalar());

                for (int i = 0; i < questionIDs.Count; i++)
                {
                    SqlCommand cmdAnswer = new SqlCommand(
                        @"INSERT INTO QuizAttemptAnswers (AttemptID, QuestionID, SelectedOption, IsCorrect)
                          VALUES (@attemptID, @questionID, @selectedOption, @isCorrect)",
                        con, trans);

                    cmdAnswer.Parameters.AddWithValue("@attemptID", attemptID);
                    cmdAnswer.Parameters.AddWithValue("@questionID", questionIDs[i]);
                    cmdAnswer.Parameters.AddWithValue("@selectedOption", selectedOptions[i]);
                    cmdAnswer.Parameters.AddWithValue("@isCorrect", correctFlags[i]);

                    cmdAnswer.ExecuteNonQuery();
                }

                if (score >= 4)
                {
                    SqlCommand cmdProgress = new SqlCommand(
                        @"UPDATE StudentProgress
                          SET Status = 'Completed',
                              CompletionDate = GETDATE(),
                              QuizScore = @score,
                              QuizPassed = 1,
                              LastQuizAttemptDate = GETDATE()
                          WHERE UserID = @userID AND LessonID = @lessonID",
                        con, trans);

                    cmdProgress.Parameters.AddWithValue("@userID", userID);
                    cmdProgress.Parameters.AddWithValue("@lessonID", lessonID);
                    cmdProgress.Parameters.AddWithValue("@score", score);

                    cmdProgress.ExecuteNonQuery();
                }
                else
                {
                    SqlCommand cmdProgress = new SqlCommand(
                        @"UPDATE StudentProgress
                          SET Status = 'Not Started',
                              CompletionDate = NULL,
                              QuizScore = @score,
                              QuizPassed = 0,
                              LastQuizAttemptDate = GETDATE()
                          WHERE UserID = @userID AND LessonID = @lessonID",
                        con, trans);

                    cmdProgress.Parameters.AddWithValue("@userID", userID);
                    cmdProgress.Parameters.AddWithValue("@lessonID", lessonID);
                    cmdProgress.Parameters.AddWithValue("@score", score);

                    cmdProgress.ExecuteNonQuery();
                }

                trans.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null && con.State == ConnectionState.Open)
                    {
                        trans.Rollback();
                    }
                }
                catch
                {
                }

                lblMessage.Text = "Error: " + ex.Message;
                return;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            Response.Redirect("QuizReview.aspx?LessonID=" + lessonID);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
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
    }
}