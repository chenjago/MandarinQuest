using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLoginSubmit_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim().ToLower();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                Response.Write("<script>alert('Please enter email and password');</script>");
                return;
            }

            string cs = ConfigurationManager
                        .ConnectionStrings["MandarinQuestDB"]
                        .ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT U.UserID, U.PasswordHash, R.RoleName
                    FROM Users U
                    INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                    INNER JOIN Roles R ON UR.RoleID = R.RoleID
                    WHERE LOWER(U.Email) = @email";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@email", email);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedHash = reader["PasswordHash"].ToString();

                        if (PasswordHelper.VerifyPassword(password, storedHash))
                        {
                            Session["UserID"] = reader["UserID"].ToString();
                            Session["Role"] = reader["RoleName"].ToString();

                            string role = reader["RoleName"].ToString().ToLower();

                            switch (role)
                            {
                                case "admin":
                                    Response.Redirect("AdminPage.aspx");
                                    break;

                                case "teacher":
                                    Response.Redirect("TeacherPage.aspx");
                                    break;

                                default:
                                    Response.Redirect("StudentPage.aspx");
                                    break;
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Invalid email or password');</script>");
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Invalid email or password');</script>");
                    }
                }
            }
        }

        protected void btnGoRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}