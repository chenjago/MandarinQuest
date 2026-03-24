using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class TeacherPage : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "teacher")
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                lblWelcomeTeacher.Text = Session["UserID"].ToString();
                LoadStats();
                LoadUpcomingSessions();
            }

        }

        void LoadStats()
        {
            con.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Levels WHERE Status='Active'", con);
            lblClassCount.Text = cmd1.ExecuteScalar().ToString();

            SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Lessons", con);
            lblLessonCount.Text = cmd2.ExecuteScalar().ToString();

            SqlCommand cmd3 = new SqlCommand(@"
                SELECT COUNT(*) 
                FROM Users U
                INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                INNER JOIN Roles R ON UR.RoleID = R.RoleID
                WHERE R.RoleName = 'student'
            ", con);
            lblStudentCount.Text = cmd3.ExecuteScalar().ToString();

            con.Close();
        }

        void LoadUpcomingSessions()
        {

            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT 
                L.LevelName,
                CS.SessionTitle,
                CS.SessionDate,
                CS.SessionLink
                FROM ClassSessions CS
                JOIN Levels L ON CS.LevelID = L.LevelID
                WHERE CAST(CS.SessionDate AS DATETIME) >= DATEADD(HOUR,-3, GETDATE())
                ORDER BY CS.SessionDate",
                con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptSessions.DataSource = dt;
            rptSessions.DataBind();

        }

        protected void btnManageLevels_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageLevels.aspx");
        }

        protected void btnManageLessons_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageLessons.aspx");
        }

        protected void btnUploadMaterials_Click(object sender, EventArgs e)
        {
            Response.Redirect("UploadMaterials.aspx");
        }

        protected void btnScheduleSessions_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScheduleSessions.aspx");
        }

        protected void btnLogoutInstructor_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("GuestPage.aspx");
        }

    }
}