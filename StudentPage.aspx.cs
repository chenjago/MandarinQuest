using MandarinQuest.Services;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

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
                return;
            }

            if (!IsPostBack)
            {
                LoadLevels();
                LoadUpcomingSessions();

                if (Session["ChatHistory"] != null)
                {
                    litChatHistory.Text = Session["ChatHistory"].ToString();
                }
            }
        }

        void LoadLevels()
        {
            int unlockedLevel = GetStudentUnlockedLevel();

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LevelID, LevelName, Description, LevelOrder FROM Levels WHERE Status='Active' ORDER BY LevelOrder",
            con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dt.Columns.Add("Locked", typeof(bool));

            foreach (DataRow row in dt.Rows)
            {
                int order = Convert.ToInt32(row["LevelOrder"]);
                row["Locked"] = order > unlockedLevel;
            }

            rptLevels.DataSource = dt;
            rptLevels.DataBind();
        }

        void LoadUpcomingSessions()
        {
            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT CS.SessionTitle, CS.SessionDate, CS.SessionLink, L.LevelName
              FROM ClassSessions CS
              JOIN Levels L ON CS.LevelID = L.LevelID
              WHERE CAST(CS.SessionDate AS DATETIME) >= DATEADD(HOUR,-3,GETDATE())
              ORDER BY CS.SessionDate",
              con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptSessions.DataSource = dt;
            rptSessions.DataBind();

            lblNoSession.Visible = dt.Rows.Count == 0;
        }

        int GetStudentUnlockedLevel()
        {
            SqlCommand cmd = new SqlCommand(@"
            SELECT ISNULL(MAX(CompletedLevels.LevelOrder), 0)
            FROM
            (
                SELECT L.LevelID, L.LevelOrder
                FROM Levels L
                INNER JOIN Lessons LS ON LS.LevelID = L.LevelID
                LEFT JOIN StudentProgress SP
                    ON SP.LessonID = LS.LessonID
                   AND SP.UserID = @u
                WHERE L.Status = 'Active'
                GROUP BY L.LevelID, L.LevelOrder
                HAVING COUNT(LS.LessonID) = SUM(CASE WHEN SP.Status = 'Completed' THEN 1 ELSE 0 END)
            ) AS CompletedLevels", con);

            cmd.Parameters.AddWithValue("@u", Session["UserID"]);

            con.Open();
            int level = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return level + 1;
        }

        protected void Level_Click(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            Response.Redirect("LessonView.aspx?levelId=" + e.CommandArgument);
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
            RegisterAsyncTask(new PageAsyncTask(HandleAskAIAsync));
        }

        private async Task HandleAskAIAsync()
        {
            string question = txtQuestion.Text.Trim();

            if (string.IsNullOrWhiteSpace(question))
                return;

            pnlWelcome.Visible = false;

            string safeQuestion = HttpUtility.HtmlEncode(question);

            litChatHistory.Text += "<div class='msg user'><div class='bubble userBubble'>" + safeQuestion + "</div></div>";
            litChatHistory.Text += "<div class='msg ai'><div class='bubble aiBubble'>Thinking...</div></div>";

            GeminiService gemini = new GeminiService();
            string aiReply = await gemini.AskGeminiAsync(question);

            string safeReply = HttpUtility.HtmlEncode(aiReply)
                .Replace("**", "")
                .Replace("\n", "<br/>");

            safeReply = safeReply
                .Replace("**", "")
                .Replace("* ", "")
                .Replace("\n", "<br/>");

            litChatHistory.Text = litChatHistory.Text.Replace(
                "<div class='msg ai'><div class='bubble aiBubble'>Thinking...</div></div>",
                "<div class='msg ai'><div class='bubble aiBubble'>" + safeReply + "</div></div>"
            );

            Session["ChatHistory"] = litChatHistory.Text;

            ScriptManager.RegisterStartupScript(this, GetType(), "openChat", "openChat();", true);

            txtQuestion.Text = "";
        }
    }
}