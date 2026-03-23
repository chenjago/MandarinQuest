using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class Profile : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadProfile();
            }
        }

        void LoadProfile()
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
            "SELECT FullName, Email FROM Users WHERE UserID=@uid", con);

            cmd.Parameters.AddWithValue("@uid", Session["UserID"]);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtProfileName.Text = dr["FullName"].ToString();
                txtProfileEmail.Text = dr["Email"].ToString();
            }

            dr.Close();
            con.Close();
        }

        protected void btnUpdateProfile_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Users SET FullName=@n WHERE UserID=@uid", con);

            cmd.Parameters.AddWithValue("@n", txtProfileName.Text);
            cmd.Parameters.AddWithValue("@uid", Session["UserID"]);

            cmd.ExecuteNonQuery();
            con.Close();

            lblMessage.Text = "Profile updated successfully.";
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {

            if (txtNewPass.Text != txtConfirmPass.Text)
            {
                lblMessage.Text = "New passwords do not match.";
                return;
            }

            con.Open();

            SqlCommand check = new SqlCommand(
            "SELECT COUNT(*) FROM Users WHERE UserID=@uid AND PasswordHash=@p", con);

            check.Parameters.AddWithValue("@uid", Session["UserID"]);
            check.Parameters.AddWithValue("@p", txtCurrentPass.Text);

            int count = Convert.ToInt32(check.ExecuteScalar());

            if (count == 0)
            {
                lblMessage.Text = "Current password incorrect.";
                con.Close();
                return;
            }

            SqlCommand update = new SqlCommand(
            "UPDATE Users SET PasswordHash=@p WHERE UserID=@uid", con);

            update.Parameters.AddWithValue("@p", txtNewPass.Text);
            update.Parameters.AddWithValue("@uid", Session["UserID"]);

            update.ExecuteNonQuery();

            con.Close();

            lblMessage.Text = "Password changed successfully.";
        }

        protected void btnBackDashboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentPage.aspx");
        }

    }
}