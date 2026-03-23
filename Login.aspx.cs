using System;
using System.Configuration;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Login : System.Web.UI.Page
    {
        protected void btnLoginSubmit_Click(object sender, EventArgs e)
        {
            // 1️⃣ Get connection string from web.config
            string cs = ConfigurationManager
                        .ConnectionStrings["MandarinQuestDB"]
                        .ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                // 2️⃣ SQL with role join + safe email matching
                string query = @"
                    SELECT U.UserID, R.RoleName
                    FROM Users U
                    INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                    INNER JOIN Roles R ON UR.RoleID = R.RoleID
                    WHERE LOWER(U.Email) = @email
                      AND U.PasswordHash = @password";

                SqlCommand cmd = new SqlCommand(query, con);

                // 3️⃣ Normalize input
                cmd.Parameters.AddWithValue("@email",
                    txtEmail.Text.Trim().ToLower());

                cmd.Parameters.AddWithValue("@password",
                    txtPassword.Text.Trim());

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                // 4️⃣ Login success
                if (reader.Read())
                {
                    Session["UserID"] = reader["UserID"].ToString();
                    Session["Role"] = reader["RoleName"].ToString();

                    string role = reader["RoleName"].ToString();

                    // 5️⃣ Role-based redirect
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
                    // 6️⃣ Login failed
                    Response.Write("<script>alert('Invalid email or password');</script>");
                }
            }
        }

        protected void btnGoRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}
