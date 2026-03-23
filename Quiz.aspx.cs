using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Quiz : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        string correctAnswer;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadQuestion();
            }
        }

        void LoadQuestion()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
            "SELECT TOP 1 * FROM QuizQuestions", con);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                lblQuestion.Text = reader["QuestionText"].ToString();

                rblAnswers.Items.Add(reader["OptionA"].ToString());
                rblAnswers.Items.Add(reader["OptionB"].ToString());
                rblAnswers.Items.Add(reader["OptionC"].ToString());
                rblAnswers.Items.Add(reader["OptionD"].ToString());

                correctAnswer = reader["CorrectAnswer"].ToString();
                ViewState["Correct"] = correctAnswer;
            }

            con.Close();
        }

        protected void btnSubmitQuiz_Click(object sender, EventArgs e)
        {
            string answer = rblAnswers.SelectedItem.Text;
            string correct = ViewState["Correct"].ToString();

            if (answer.StartsWith(correct))
            {
                lblResult.Text = "Correct!";
            }
            else
            {
                lblResult.Text = "Wrong Answer";
            }
        }
    }
}