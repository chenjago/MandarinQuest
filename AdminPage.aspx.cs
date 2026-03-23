using System;

namespace MandarinQuest
{
    public partial class AdminPage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnManageUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUsers.aspx");
        }

        protected void btnManageRoles_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageRoles.aspx");
        }

        protected void btnViewReports_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reports.aspx");
        }

        protected void btnAuditLogs_Click(object sender, EventArgs e)
        {
            Response.Redirect("AuditLogs.aspx");
        }

        protected void btnLogoutAdmin_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

    }
}