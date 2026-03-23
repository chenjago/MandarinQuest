using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class ViewReports : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
                LoadStats();

        }

        void LoadStats()
        {

            con.Open();

            SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM Users", con);
            lblUsers.Text = cmd1.ExecuteScalar().ToString();

            SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM Lessons", con);
            lblLessons.Text = cmd2.ExecuteScalar().ToString();

            SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM LearningMaterials", con);
            lblMaterials.Text = cmd3.ExecuteScalar().ToString();

            con.Close();

        }

    }
}