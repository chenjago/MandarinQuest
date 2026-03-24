using System;
using System.Web;
using System.Web.UI;

namespace MandarinQuest
{
    public partial class AdminPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnManageUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUsers.aspx");
        }

        protected void btnCreateUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateUsers.aspx");
        }

        protected void btnViewReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reports.aspx");
        }

        protected void btnTeachingDashboard_Click(object sender, EventArgs e)
        {
            Session["FromAdmin"] = true;
            Response.Redirect("TeacherPage.aspx");
        }

        protected void btnAuditLogs_Click(object sender, EventArgs e)
        {
            Response.Redirect("AuditLogs.aspx");
        }

        protected void btnLogoutAdmin_Click(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("Login.aspx");
        }
    }
}