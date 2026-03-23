using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class ProgressTracking : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "student")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadStudentLevel();
                LoadProgress();
                LoadStats();
                LoadOverallProgress();
            }

        }

        void LoadStudentLevel()
        {

            SqlCommand cmd = new SqlCommand(
            @"SELECT TOP 1 L.LevelName
              FROM StudentProgress SP
              JOIN Lessons LS ON SP.LessonID = LS.LessonID
              JOIN Levels L ON LS.LevelID = L.LevelID
              WHERE SP.UserID=@uid AND SP.Status='Completed'
              ORDER BY L.LevelOrder DESC", con);

            cmd.Parameters.AddWithValue("@uid", Session["UserID"]);

            con.Open();

            object result = cmd.ExecuteScalar();

            con.Close();

            if (result != null)
                lblCurrentLevel.Text = result.ToString();
            else
                lblCurrentLevel.Text = "Beginner";

        }

        void LoadProgress()
        {

            string q = @"SELECT Lessons.LessonTitle, StudentProgress.Status, StudentProgress.CompletionDate
                         FROM StudentProgress
                         JOIN Lessons ON StudentProgress.LessonID = Lessons.LessonID
                         WHERE StudentProgress.UserID = @uid";

            SqlDataAdapter da = new SqlDataAdapter(q, con);

            da.SelectCommand.Parameters.AddWithValue("@uid", Session["UserID"]);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvProgress.DataSource = dt;
            dgvProgress.DataBind();

        }

        void LoadStats()
        {

            con.Open();

            SqlCommand cmd1 = new SqlCommand(
            "SELECT COUNT(*) FROM StudentProgress WHERE UserID=@uid AND Status='Completed'", con);

            cmd1.Parameters.AddWithValue("@uid", Session["UserID"]);
            lblCompleted.Text = cmd1.ExecuteScalar().ToString();


            SqlCommand cmd2 = new SqlCommand(
            "SELECT COUNT(*) FROM StudentProgress WHERE UserID=@uid AND Status='In Progress'", con);

            cmd2.Parameters.AddWithValue("@uid", Session["UserID"]);
            lblInProgress.Text = cmd2.ExecuteScalar().ToString();


            SqlCommand cmd3 = new SqlCommand(
            "SELECT COUNT(*) FROM Lessons", con);

            lblTotal.Text = cmd3.ExecuteScalar().ToString();

            con.Close();

        }

        void LoadOverallProgress()
        {

            con.Open();

            SqlCommand cmdTotal = new SqlCommand(
            "SELECT COUNT(*) FROM Lessons", con);

            int totalLessons = Convert.ToInt32(cmdTotal.ExecuteScalar());


            SqlCommand cmdCompleted = new SqlCommand(
            "SELECT COUNT(*) FROM StudentProgress WHERE UserID=@uid AND Status='Completed'", con);

            cmdCompleted.Parameters.AddWithValue("@uid", Session["UserID"]);

            int completed = Convert.ToInt32(cmdCompleted.ExecuteScalar());

            con.Close();

            int percent = 0;

            if (totalLessons > 0)
                percent = (completed * 100) / totalLessons;

            lblProgressPercent.Text = percent + "% Completed";

            progressFill.Style["width"] = percent + "%";

        }

        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentPage.aspx");
        }

    }
}