using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class MyClasses : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadClasses();
        }

        void LoadClasses()
        {
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT ClassName, Description FROM Classes", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvClasses.DataSource = dt;
            dgvClasses.DataBind();
        }
    }
}