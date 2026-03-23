using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class DisableAccount : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        "Data Source=.;Initial Catalog=MandarinQuestDB;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text;

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Users SET IsActive = 0 WHERE Email=@Email OR FullName=@Name", con);

            cmd.Parameters.AddWithValue("@Email", user);
            cmd.Parameters.AddWithValue("@Name", user);

            int rows = cmd.ExecuteNonQuery();

            con.Close();

            if (rows > 0)
            {
                lblMessage.Text = "User account disabled successfully.";
            }
            else
            {
                lblMessage.Text = "User not found.";
            }
        }

    }
}