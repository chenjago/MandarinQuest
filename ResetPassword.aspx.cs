using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class ResetPassword : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void btnReset_Click(object sender, EventArgs e)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Users SET PasswordHash=@p WHERE UserID=@id", con);

            cmd.Parameters.AddWithValue("@p", txtPassword.Text);
            cmd.Parameters.AddWithValue("@id", txtUserID.Text);

            cmd.ExecuteNonQuery();

            con.Close();

        }

    }
}