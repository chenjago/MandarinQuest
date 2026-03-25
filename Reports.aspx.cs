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
                LoadStudentProgress();
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

            SqlCommand cmdLessons = new SqlCommand(
                "SELECT COUNT(*) FROM Lessons", con);
            lblLessons.Text = cmdLessons.ExecuteScalar().ToString();

            SqlCommand cmdMaterials = new SqlCommand(
                "SELECT COUNT(*) FROM LearningMaterials", con);
            lblMaterials.Text = cmdMaterials.ExecuteScalar().ToString();

            con.Close();
        }

        void LoadRegistrations()
        {
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
        }

        void LoadStudentProgress()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT 
                U.FullName AS StudentName,
                ISNULL(SUM(CASE WHEN SP.Status = 'Completed' THEN 1 ELSE 0 END), 0) AS CompletedLessons,
                ISNULL(SUM(CASE WHEN SP.Status = 'In Progress' THEN 1 ELSE 0 END), 0) AS InProgressLessons,
                (SELECT COUNT(*) FROM Lessons) AS TotalLessons,
                CAST(
                    ISNULL(SUM(CASE WHEN SP.Status = 'Completed' THEN 1 ELSE 0 END), 0) * 100.0 /
                    NULLIF((SELECT COUNT(*) FROM Lessons), 0)
                AS DECIMAL(5,2)) AS Percentage
              FROM Users U
              INNER JOIN UserRoles UR ON U.UserID = UR.UserID
              LEFT JOIN StudentProgress SP ON U.UserID = SP.UserID
              WHERE UR.RoleID = 2
              GROUP BY U.UserID, U.FullName
              ORDER BY U.FullName", 
              con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvStudentProgress.DataSource = dt;
            gvStudentProgress.DataBind();
        }

        void LoadCourseProgress()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT 
            L.LessonTitle,
            COUNT(DISTINCT CASE WHEN SP.Status = 'Completed' THEN SP.UserID END) AS StudentsCompleted,
            COUNT(DISTINCT CASE WHEN SP.Status = 'In Progress' THEN SP.UserID END) AS StudentsInProgress,
            ISNULL(
                CAST(
                    COUNT(DISTINCT CASE WHEN SP.Status = 'Completed' THEN SP.UserID END) * 100.0 /
                    NULLIF((SELECT COUNT(DISTINCT UserID) FROM UserRoles WHERE RoleID = 2), 0)
                AS DECIMAL(5,2)),
            0) AS Percentage
          FROM Lessons L
          LEFT JOIN StudentProgress SP ON L.LessonID = SP.LessonID
          GROUP BY L.LessonID, L.LessonTitle
          ORDER BY L.LessonID", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvCourseProgress.DataSource = dt;
            gvCourseProgress.DataBind();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}