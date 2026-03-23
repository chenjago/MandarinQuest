using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class ViewClasses : System.Web.UI.Page
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

            SqlDataAdapter da =
            new SqlDataAdapter("SELECT * FROM ClassSessions", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvClasses.DataSource = dt;
            gvClasses.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            string query =
            "SELECT * FROM Classes WHERE 1=1";

            if (txtSearch.Text != "")
                query += " AND ClassName LIKE '%" + txtSearch.Text + "%'";

            if (ddlStatus.SelectedValue != "")
                query += " AND Status='" + ddlStatus.SelectedValue + "'";

            if (txtDate.Text != "")
                query += " AND CAST(ClassDate AS DATE)='" + txtDate.Text + "'";

            SqlDataAdapter da = new SqlDataAdapter(query, con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvClasses.DataSource = dt;
            gvClasses.DataBind();

        }

    }
}