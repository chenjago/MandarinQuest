using System;

namespace MandarinQuest
{
    public partial class GuestPage : System.Web.UI.Page
    {

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

    }
}