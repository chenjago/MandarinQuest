using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Register : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void btnRegisterSubmit_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand checkEmail = new SqlCommand(
            "SELECT COUNT(*) FROM Users WHERE Email=@e", con);

            checkEmail.Parameters.AddWithValue("@e", txtRegEmail.Text.Trim().ToLower());

            int exists = (int)checkEmail.ExecuteScalar();

            if (exists > 0)
            {
                Response.Write("<script>alert('Email already exists');</script>");
                con.Close();
                return;
            }

            // 1️⃣ Insert user
            SqlCommand cmdUser = new SqlCommand(
            "INSERT INTO Users (FullName, Email, PasswordHash) OUTPUT INSERTED.UserID VALUES (@n,@e,@p)", con);

            cmdUser.Parameters.AddWithValue("@n", txtFullName.Text);
            cmdUser.Parameters.AddWithValue("@e", txtRegEmail.Text.Trim().ToLower());
            cmdUser.Parameters.AddWithValue("@p", txtRegPassword.Text);

            int newUserID = (int)cmdUser.ExecuteScalar();

            // 2️⃣ Get Student RoleID
            SqlCommand cmdRole = new SqlCommand(
            "SELECT RoleID FROM Roles WHERE RoleName='Student'", con);

            int roleID = (int)cmdRole.ExecuteScalar();

            // 3️⃣ Assign role to user
            SqlCommand cmdUserRole = new SqlCommand(
            "INSERT INTO UserRoles (UserID, RoleID) VALUES (@uid,@rid)", con);

            cmdUserRole.Parameters.AddWithValue("@uid", newUserID);
            cmdUserRole.Parameters.AddWithValue("@rid", roleID);
            cmdUserRole.ExecuteNonQuery();

            con.Close();

            Response.Redirect("Login.aspx");
        }

        protected void btnBackLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}
