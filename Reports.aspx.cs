using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class Reports : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

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
            using (SqlConnection con = new SqlConnection(cs))
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
            }
        }

        void LoadRegistrations()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT 
                        DATENAME(MONTH, U.CreatedDate) + ' ' + CAST(YEAR(U.CreatedDate) AS VARCHAR) AS Month,
                        COUNT(DISTINCT U.UserID) AS Count
                      FROM Users U
                      INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                      WHERE UR.RoleID = 2
                      GROUP BY YEAR(U.CreatedDate), MONTH(U.CreatedDate), DATENAME(MONTH, U.CreatedDate)
                      ORDER BY YEAR(U.CreatedDate), MONTH(U.CreatedDate)", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRegistrations.DataSource = dt;
                gvRegistrations.DataBind();
            }
        }

        void LoadStudentProgress()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT 
                        U.FullName AS StudentName,
                        ISNULL(SUM(CASE WHEN SP.Status = 'Completed' THEN 1 ELSE 0 END), 0) AS CompletedLessons,
                        ISNULL(SUM(CASE WHEN SP.Status = 'In Progress' THEN 1 ELSE 0 END), 0) AS InProgressLessons,
                        (SELECT COUNT(*) FROM Lessons) AS TotalLessons,
                        ISNULL(
                            CAST(
                                ISNULL(SUM(CASE WHEN SP.Status = 'Completed' THEN 1 ELSE 0 END), 0) * 100.0 /
                                NULLIF((SELECT COUNT(*) FROM Lessons), 0)
                            AS DECIMAL(5,2)),
                        0) AS Percentage
                      FROM Users U
                      INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                      LEFT JOIN StudentProgress SP ON U.UserID = SP.UserID
                      WHERE UR.RoleID = 2
                      GROUP BY U.UserID, U.FullName
                      ORDER BY U.FullName", con);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvStudentProgress.DataSource = dt;
                gvStudentProgress.DataBind();
            }
        }

        void LoadCourseProgress()
        {
            using (SqlConnection con = new SqlConnection(cs))
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
        }

        protected void gvStudentProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfPercentage = (HiddenField)e.Row.FindControl("hfStudentPercentage");
                Panel pnlProgress = (Panel)e.Row.FindControl("pnlStudentProgress");
                Label lblProgress = (Label)e.Row.FindControl("lblStudentProgressText");

                decimal percentage = ParsePercentage(hfPercentage.Value);
                string text = percentage.ToString("0.##", CultureInfo.InvariantCulture) + "%";

                pnlProgress.Width = Unit.Percentage((double)percentage);
                lblProgress.Text = text;
            }
        }

        protected void gvCourseProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfPercentage = (HiddenField)e.Row.FindControl("hfCoursePercentage");
                Panel pnlProgress = (Panel)e.Row.FindControl("pnlCourseProgress");
                Label lblProgress = (Label)e.Row.FindControl("lblCourseProgressText");

                decimal percentage = ParsePercentage(hfPercentage.Value);
                string text = percentage.ToString("0.##", CultureInfo.InvariantCulture) + "%";

                pnlProgress.Width = Unit.Percentage((double)percentage);
                lblProgress.Text = text;
            }
        }

        decimal ParsePercentage(string value)
        {
            decimal result = 0;
            decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

            if (result < 0)
                result = 0;

            if (result > 100)
                result = 100;

            return result;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}