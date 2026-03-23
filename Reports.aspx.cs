using System;

namespace MandarinQuest
{
    public partial class Reports : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReports();
            }
        }

        void LoadReports()
        {

            // Sample data (no database)

            int totalUsers = 12;
            int totalRoles = 3;
            int totalLessons = 18;
            int totalClasses = 7;
            int totalStudents = 9;
            int totalTeachers = 3;

            lblUsers.Text = totalUsers.ToString();
            lblRoles.Text = totalRoles.ToString();
            lblLessons.Text = totalLessons.ToString();
            lblClasses.Text = totalClasses.ToString();
            lblStudents.Text = totalStudents.ToString();
            lblTeachers.Text = totalTeachers.ToString();

            lblStatus.Text = "System Running Normally";
        }

    }
}