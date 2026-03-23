using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class StudentPage : System.Web.UI.Page
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
                LoadLevels();
                LoadUpcomingSessions();
            }

        }

        void LoadLevels()
        {

            int unlockedLevel = GetStudentUnlockedLevel();

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LevelID,LevelName,Description,LevelOrder FROM Levels WHERE Status='Active' ORDER BY LevelOrder",
            con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("Locked", typeof(bool));

            foreach (DataRow row in dt.Rows)
            {

                int order = Convert.ToInt32(row["LevelOrder"]);

                if (order > unlockedLevel)
                    row["Locked"] = true;
                else
                    row["Locked"] = false;

            }

            rptLevels.DataSource = dt;
            rptLevels.DataBind();

        }

        void LoadUpcomingSessions()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT CS.SessionTitle, CS.SessionDate, CS.SessionLink, L.LevelName
              FROM ClassSessions CS
              JOIN Levels L ON CS.LevelID=L.LevelID
              WHERE CAST(CS.SessionDate AS DATETIME) >= DATEADD(HOUR,-3,GETDATE())
              ORDER BY CS.SessionDate",
              con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptSessions.DataSource = dt;
            rptSessions.DataBind();

            if (dt.Rows.Count == 0)
                lblNoSession.Visible = true;

        }

        int GetStudentUnlockedLevel()
        {

            SqlCommand cmd = new SqlCommand(
            @"SELECT ISNULL(MAX(L.LevelOrder),0)
            FROM StudentProgress SP
            JOIN Lessons LS ON SP.LessonID=LS.LessonID
            JOIN Levels L ON LS.LevelID=L.LevelID
            WHERE SP.UserID=@u AND SP.Status='Completed'", con);

            cmd.Parameters.AddWithValue("@u", Session["UserID"]);

            con.Open();

            int level = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            return level + 1;

        }

        protected void Level_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            Response.Redirect("LessonView.aspx?level=" + e.CommandArgument);

        }

        protected void btnLessons_Click(object sender, EventArgs e)
        {
            Response.Redirect("LessonView.aspx");
        }

        protected void btnMaterials_Click(object sender, EventArgs e)
        {
            Response.Redirect("LearningMaterials.aspx");
        }

        protected void btnProgress_Click(object sender, EventArgs e)
        {
            Response.Redirect("ProgressTracking.aspx");
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("GuestPage.aspx");
        }

        protected void btnAskAI_Click(object sender, EventArgs e)
        {

            string question = txtQuestion.Text;

            string response = "Try this Mandarin phrase: 你好 (Hello)";

            litChatHistory.Text += "<div><b>You:</b> " + question + "</div>";
            litChatHistory.Text += "<div style='color:#a11212'><b>AI:</b> " + response + "</div>";

            txtQuestion.Text = "";

        }

    }
}