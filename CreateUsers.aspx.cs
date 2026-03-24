using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class CreateUsers : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadRoles();
            }
        }

        private void LoadRoles()
        {
            ddlRoles.Items.Clear();

            // Default option
            ddlRoles.Items.Add(new ListItem("-- Select Role --", ""));

            con.Open();

            // Only allow Teacher & Student (adjust RoleID if needed)
            SqlCommand cmd = new SqlCommand(
                "SELECT RoleID, RoleName FROM Roles WHERE RoleName IN ('Teacher', 'Student')",
                con);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ddlRoles.Items.Add(
                    new ListItem(
                        reader["RoleName"].ToString(),
                        reader["RoleID"].ToString()
                    )
                );
            }

            reader.Close();
            con.Close();
        }

        protected void btnCreateUser_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string selectedRoleID = ddlRoles.SelectedValue;

            // Validation
            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(selectedRoleID))
            {
                lblMessage.Text = "All fields are required and role must be selected.";
                lblMessage.CssClass = "message error";
                return;
            }

            int adminId = (Session["AdminUserID"] != null) ? Convert.ToInt32(Session["AdminUserID"]) : 0;

            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                // Insert User
                SqlCommand cmdUser = new SqlCommand(
                    @"INSERT INTO Users (FullName, Email, PasswordHash, CreatedDate) 
                      VALUES (@fullName, @Email, @PasswordHash, @CreatedDate); 
                      SELECT SCOPE_IDENTITY();",
                    con, transaction
                );

                cmdUser.Parameters.AddWithValue("@fullName", fullName);
                cmdUser.Parameters.AddWithValue("@Email", email);
                cmdUser.Parameters.AddWithValue("@PasswordHash", password);
                cmdUser.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                int newUserID = Convert.ToInt32(cmdUser.ExecuteScalar());

                // Assign Role
                SqlCommand cmdRole = new SqlCommand(
                    "INSERT INTO UserRoles (UserID, RoleID) VALUES (@UserID, @RoleID)",
                    con, transaction
                );

                cmdRole.Parameters.AddWithValue("@UserID", newUserID);
                cmdRole.Parameters.AddWithValue("@RoleID", selectedRoleID);
                cmdRole.ExecuteNonQuery();

                // Audit Log
                AddAuditLog(newUserID, "CreateUser", $"Created user '{fullName}' with email '{email}'", adminId);

                transaction.Commit();

                lblMessage.Text = "User created successfully!";
                lblMessage.CssClass = "message";

                // Reset fields
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                ddlRoles.SelectedIndex = 0;
            }
            catch
            {
                transaction.Rollback();
                lblMessage.Text = "Error creating user.";
                lblMessage.CssClass = "message error";
            }
            finally
            {
                con.Close();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }

        private void AddAuditLog(int affectedUserId, string action, string description, int performedBy)
        {
            using (SqlConnection conn = new SqlConnection(con.ConnectionString))
            {
                string query = @"INSERT INTO AuditLogs 
                                 (UserID, Action, Description, LogDate, PerformedBy) 
                                 VALUES (@userId, @action, @desc, GETDATE(), @performedBy)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", affectedUserId);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@performedBy", performedBy);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}