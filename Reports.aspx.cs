using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Reports : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
              Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStats();
                LoadRegistrations();
                LoadCourseProgress();
            }
        }

        void LoadStats()
        {
            con.Open();

            SqlCommand cmdTeachers = new SqlCommand(
                "SELECT COUNT(DISTINCT UserID) FROM UserRoles WHERE RoleID = 3", con);
            lblTeachers.Text = cmdTeachers.ExecuteScalar().ToString();

            SqlCommand cmdStudents = new SqlCommand(
                "SELECT COUNT(DISTINCT UserID) FROM UserRoles WHERE RoleID = 2", con);
            lblStudents.Text = cmdStudents.ExecuteScalar().ToString();

            SqlCommand cmdLessons = new SqlCommand("SELECT COUNT(*) FROM Lessons", con);
            lblLessons.Text = cmdLessons.ExecuteScalar().ToString();

            SqlCommand cmdMaterials = new SqlCommand("SELECT COUNT(*) FROM LearningMaterials", con);
            lblMaterials.Text = cmdMaterials.ExecuteScalar().ToString();

            con.Close();
        }

        void LoadRegistrations()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT DATENAME(MONTH, CreatedDate) + ' ' + CAST(YEAR(CreatedDate) AS VARCHAR) AS Month,
                         COUNT(UserID) AS Count
                  FROM Users
                  GROUP BY YEAR(CreatedDate), MONTH(CreatedDate), DATENAME(MONTH, CreatedDate)
                  ORDER BY YEAR(CreatedDate), MONTH(CreatedDate)", con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            gvRegistrations.DataSource = dt;
            gvRegistrations.DataBind();
            con.Close();
        }
        void LoadCourseProgress()
        {
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT sp.LessonID AS Lesson,
                 COUNT(sp.UserID) AS StudentsCompleted,
                 (COUNT(sp.UserID) * 100.0 / 
                    (SELECT COUNT(*) FROM UserRoles WHERE RoleID = 2)) AS Percentage
          FROM StudentProgress sp
          WHERE sp.Status = 'Completed'
          GROUP BY sp.LessonID
          ORDER BY sp.LessonID", con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            gvCourseProgress.DataSource = dt;
            gvCourseProgress.DataBind();
            con.Close();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}