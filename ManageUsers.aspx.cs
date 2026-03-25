using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString().ToLower() != "admin")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadRoles();
                LoadUsers("");
            }
        }

        void LoadRoles()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT RoleID, RoleName FROM Roles ORDER BY RoleName";
                SqlDataAdapter da = new SqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlFilterRole.DataSource = dt;
                ddlFilterRole.DataTextField = "RoleName";
                ddlFilterRole.DataValueField = "RoleName";
                ddlFilterRole.DataBind();

                ddlFilterRole.Items.Insert(0, new ListItem("All Roles", ""));
            }
        }

        void LoadUsers(string roleFilter, string keyword = "")
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"
                    SELECT 
                        U.UserID,
                        U.FullName,
                        U.Email,
                        R.RoleName
                    FROM Users U
                    INNER JOIN UserRoles UR ON U.UserID = UR.UserID
                    INNER JOIN Roles R ON UR.RoleID = R.RoleID
                    WHERE (@role = '' OR R.RoleName = @role)
                      AND (
                            @keyword = ''
                            OR U.FullName LIKE '%' + @keyword + '%'
                            OR U.Email LIKE '%' + @keyword + '%'
                          )
                    ORDER BY U.UserID";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@role", roleFilter ?? "");
                cmd.Parameters.AddWithValue("@keyword", keyword ?? "");

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        protected void ddlFilterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlFilterRole.SelectedIndex = 0;
            LoadUsers("", "");
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvUsers.Rows[e.RowIndex];

            TextBox txtFullName = row.FindControl("txtFullName") as TextBox;
            TextBox txtEmail = row.FindControl("txtEmail") as TextBox;
            TextBox txtPassword = row.FindControl("txtPassword") as TextBox;

            string fullName = txtFullName != null ? txtFullName.Text.Trim() : "";
            string email = txtEmail != null ? txtEmail.Text.Trim().ToLower() : "";
            string newPassword = txtPassword != null ? txtPassword.Text.Trim() : "";

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowAlert("Full Name cannot be empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowAlert("Email cannot be empty.");
                return;
            }

            if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 6)
            {
                ShowAlert("New password must be at least 6 characters.");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    SqlCommand checkEmail = new SqlCommand(
                        @"SELECT COUNT(*) 
                          FROM Users 
                          WHERE Email = @email AND UserID <> @id", con, trans);

                    checkEmail.Parameters.AddWithValue("@email", email);
                    checkEmail.Parameters.AddWithValue("@id", userId);

                    int exists = Convert.ToInt32(checkEmail.ExecuteScalar());

                    if (exists > 0)
                    {
                        trans.Rollback();
                        ShowAlert("Email is already used by another user.");
                        return;
                    }

                    string query = @"UPDATE Users 
                                     SET FullName = @name, Email = @email";

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        query += ", PasswordHash = @password";
                    }

                    query += " WHERE UserID = @id";

                    SqlCommand cmd = new SqlCommand(query, con, trans);
                    cmd.Parameters.AddWithValue("@name", fullName);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@id", userId);

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        string hashedPassword = PasswordHelper.HashPassword(newPassword);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                    }

                    cmd.ExecuteNonQuery();

                    int adminId = Convert.ToInt32(Session["UserID"]);

                    AddAuditLog(
                        userId,
                        "Update",
                        "Updated user profile" + (string.IsNullOrEmpty(newPassword) ? "" : " and changed password"),
                        adminId,
                        con,
                        trans
                    );

                    trans.Commit();

                    gvUsers.EditIndex = -1;
                    LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
                    ShowAlert("User updated successfully.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch
                    {
                    }

                    ShowAlert("Error updating user: " + ex.Message);
                }
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            int adminId = Convert.ToInt32(Session["UserID"]);

            if (userId == adminId)
            {
                ShowAlert("You cannot delete your own admin account.");
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlTransaction trans = con.BeginTransaction();

                try
                {
                    string fullName = "";

                    SqlCommand getNameCmd = new SqlCommand(
                        "SELECT FullName FROM Users WHERE UserID = @id", con, trans);
                    getNameCmd.Parameters.AddWithValue("@id", userId);

                    object nameObj = getNameCmd.ExecuteScalar();
                    if (nameObj != null)
                    {
                        fullName = nameObj.ToString();
                    }

                    // Delete child/dependent data first
                    SqlCommand cmd1 = new SqlCommand("DELETE FROM QuizAttemptAnswers WHERE AttemptID IN (SELECT AttemptID FROM QuizAttempts WHERE UserID = @id)", con, trans);
                    cmd1.Parameters.AddWithValue("@id", userId);
                    cmd1.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("DELETE FROM QuizAttempts WHERE UserID = @id", con, trans);
                    cmd2.Parameters.AddWithValue("@id", userId);
                    cmd2.ExecuteNonQuery();

                    SqlCommand cmd3 = new SqlCommand("DELETE FROM StudentProgress WHERE UserID = @id", con, trans);
                    cmd3.Parameters.AddWithValue("@id", userId);
                    cmd3.ExecuteNonQuery();

                    SqlCommand cmd4 = new SqlCommand("DELETE FROM UserRoles WHERE UserID = @id", con, trans);
                    cmd4.Parameters.AddWithValue("@id", userId);
                    cmd4.ExecuteNonQuery();

                    SqlCommand cmd5 = new SqlCommand("DELETE FROM Users WHERE UserID = @id", con, trans);
                    cmd5.Parameters.AddWithValue("@id", userId);
                    cmd5.ExecuteNonQuery();

                    AddAuditLog(
                        userId,
                        "Delete",
                        "Deleted user: " + fullName,
                        adminId,
                        con,
                        trans
                    );

                    trans.Commit();

                    LoadUsers(ddlFilterRole.SelectedValue, txtSearch.Text.Trim());
                    ShowAlert("User deleted successfully.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch
                    {
                    }

                    ShowAlert("Error deleting user: " + ex.Message);
                }
            }
        }

        void AddAuditLog(int targetUserId, string actionType, string description, int adminUserId, SqlConnection con, SqlTransaction trans)
        {
            SqlCommand cmd = new SqlCommand(
                @"INSERT INTO AuditLogs (UserID, ActionType, Description, ActionDate, PerformedBy)
                  VALUES (@uid, @action, @desc, @date, @performedBy)", con, trans);

            cmd.Parameters.AddWithValue("@uid", targetUserId);
            cmd.Parameters.AddWithValue("@action", actionType);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@performedBy", adminUserId);

            cmd.ExecuteNonQuery();
        }

        void ShowAlert(string message)
        {
            string safeMessage = message.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", $"alert('{safeMessage}');", true);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }
    }
}