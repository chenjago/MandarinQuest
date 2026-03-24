using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
            @"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
              Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers("All");
            }
        }

        void LoadUsers(string roleFilter = "All")
        {
            string query = @"SELECT u.UserID, u.FullName, u.Email, u.CreatedDate,
                                    r.RoleName, r.RoleID
                             FROM Users u
                             LEFT JOIN UserRoles ur ON u.UserID = ur.UserID
                             LEFT JOIN Roles r ON ur.RoleID = r.RoleID";

            if (roleFilter != "All")
            {
                query += " WHERE LOWER(r.RoleName) = LOWER(@role)";
            }

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            if (roleFilter != "All")
            {
                da.SelectCommand.Parameters.AddWithValue("@role", roleFilter);
            }

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }

        protected void ddlFilterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers(ddlFilterRole.SelectedValue);
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers(ddlFilterRole.SelectedValue);

            GridViewRow row = gvUsers.Rows[e.NewEditIndex];
            DropDownList ddlRoles = (DropDownList)row.FindControl("ddlRoles");

            if (ddlRoles != null)
            {
                ddlRoles.Enabled = false;

                string roleName = row.Cells[4].Text;

                ddlRoles.Items.Clear();
                ddlRoles.Items.Add(new ListItem(roleName, "0"));
                ddlRoles.SelectedIndex = 0;
            }
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers(ddlFilterRole.SelectedValue);
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            int userId = Convert.ToInt32(id);

            GridViewRow row = gvUsers.Rows[e.RowIndex];

            TextBox txtFullName = (TextBox)row.FindControl("txtFullName");
            TextBox txtEmail = (TextBox)row.FindControl("txtEmail");
            TextBox txtPassword = (TextBox)row.FindControl("txtPassword");

            string name = txtFullName?.Text.Trim() ?? "";
            string email = txtEmail?.Text.Trim() ?? "";
            string newPassword = txtPassword?.Text.Trim() ?? "";

            int adminId = (Session["AdminUserID"] != null)
                ? Convert.ToInt32(Session["AdminUserID"])
                : 0;

            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                string updateQuery = "UPDATE Users SET FullName=@name, Email=@email";

                if (!string.IsNullOrEmpty(newPassword))
                    updateQuery += ", PasswordHash=@password";

                updateQuery += " WHERE UserID=@id";

                SqlCommand cmd = new SqlCommand(updateQuery, con, transaction);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", userId);

                if (!string.IsNullOrEmpty(newPassword))
                    cmd.Parameters.AddWithValue("@password", newPassword);

                cmd.ExecuteNonQuery();

                AddAuditLog(userId, "Update",
                    $"Updated user '{name}', email '{email}'" +
                    (string.IsNullOrEmpty(newPassword) ? "" : ", password changed"),
                    adminId);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }

            gvUsers.EditIndex = -1;
            LoadUsers(ddlFilterRole.SelectedValue);
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            int userId = Convert.ToInt32(id);

            SqlCommand cmdCheck = new SqlCommand(
                @"SELECT r.RoleID 
                  FROM UserRoles ur 
                  JOIN Roles r ON ur.RoleID = r.RoleID 
                  WHERE ur.UserID=@id", con);

            cmdCheck.Parameters.AddWithValue("@id", userId);

            con.Open();
            object roleObj = cmdCheck.ExecuteScalar();
            con.Close();

            if (roleObj != null && roleObj.ToString() == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('Admin users cannot be deleted.');", true);
                return;
            }

            int adminId = (Session["AdminUserID"] != null)
                ? Convert.ToInt32(Session["AdminUserID"])
                : 0;

            con.Open();
            SqlTransaction transaction = con.BeginTransaction();

            try
            {
                SqlCommand cmdRoles = new SqlCommand(
                    "DELETE FROM UserRoles WHERE UserID=@id", con, transaction);
                cmdRoles.Parameters.AddWithValue("@id", userId);
                cmdRoles.ExecuteNonQuery();

                SqlCommand cmdUser = new SqlCommand(
                    "DELETE FROM Users WHERE UserID=@id", con, transaction);
                cmdUser.Parameters.AddWithValue("@id", userId);
                cmdUser.ExecuteNonQuery();

                AddAuditLog(userId, "Delete",
                    $"Deleted user with ID {userId}", adminId);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                con.Close();
            }

            LoadUsers(ddlFilterRole.SelectedValue);
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