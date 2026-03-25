using System;
using System.Data;
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
            if (!Page.IsValid)
            {
                return;
            }

            string fullName = txtFullName.Text.Trim();
            string email = txtRegEmail.Text.Trim().ToLower();
            string password = txtRegPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                lblMessage.Text = "Full Name cannot be empty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                lblMessage.Text = "Email cannot be empty.";
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Password cannot be empty.";
                return;
            }

            if (password.Length < 6)
            {
                lblMessage.Text = "Password must be at least 6 characters.";
                return;
            }

            string hashedPassword = PasswordHelper.HashPassword(password);

            SqlTransaction trans = null;

            try
            {
                con.Open();
                trans = con.BeginTransaction();

                SqlCommand checkEmail = new SqlCommand(
                "SELECT COUNT(*) FROM Users WHERE Email = @e", con, trans);

                checkEmail.Parameters.AddWithValue("@e", email);

                int exists = Convert.ToInt32(checkEmail.ExecuteScalar());

                if (exists > 0)
                {
                    lblMessage.Text = "Email already exists.";
                    trans.Rollback();
                    return;
                }

                SqlCommand cmdUser = new SqlCommand(
                @"INSERT INTO Users (FullName, Email, PasswordHash)
                  OUTPUT INSERTED.UserID
                  VALUES (@n, @e, @p)", con, trans);

                cmdUser.Parameters.AddWithValue("@n", fullName);
                cmdUser.Parameters.AddWithValue("@e", email);
                cmdUser.Parameters.AddWithValue("@p", hashedPassword);

                int newUserID = Convert.ToInt32(cmdUser.ExecuteScalar());

                SqlCommand cmdRole = new SqlCommand(
                "SELECT RoleID FROM Roles WHERE RoleName = 'Student'", con, trans);

                object roleObj = cmdRole.ExecuteScalar();

                if (roleObj == null)
                {
                    lblMessage.Text = "Student role not found.";
                    trans.Rollback();
                    return;
                }

                int roleID = Convert.ToInt32(roleObj);

                SqlCommand cmdUserRole = new SqlCommand(
                "INSERT INTO UserRoles (UserID, RoleID) VALUES (@uid, @rid)", con, trans);

                cmdUserRole.Parameters.AddWithValue("@uid", newUserID);
                cmdUserRole.Parameters.AddWithValue("@rid", roleID);
                cmdUserRole.ExecuteNonQuery();

                SqlCommand cmdProgress = new SqlCommand(
                @"INSERT INTO StudentProgress
                  (UserID, LessonID, Status, CompletionDate, QuizScore, QuizPassed, LastQuizAttemptDate)
                  SELECT
                      @userId,
                      L.LessonID,
                      'Not Started',
                      NULL,
                      NULL,
                      0,
                      NULL
                  FROM Lessons L
                  LEFT JOIN StudentProgress SP
                      ON SP.UserID = @userId
                     AND SP.LessonID = L.LessonID
                  WHERE SP.ProgressID IS NULL", con, trans);

                cmdProgress.Parameters.AddWithValue("@userId", newUserID);
                cmdProgress.ExecuteNonQuery();

                trans.Commit();
                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                try
                {
                    if (trans != null)
                    {
                        trans.Rollback();
                    }
                }
                catch
                {
                }

                lblMessage.Text = "Error: " + ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        protected void btnBackLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}