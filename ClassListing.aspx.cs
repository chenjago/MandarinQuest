using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace MandarinQuest
{
    public partial class ClassListing : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClasses();
            }
        }

        private void LoadClasses()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT ClassID, ClassName, Description FROM Classes", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvClasses.DataSource = dt;
            dgvClasses.DataBind();
        }

        protected void btnViewPreview_Click(object sender, EventArgs e)
        {
            Response.Redirect("LessonPreview.aspx");
        }

        protected void btnBackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("GuestPage.aspx");
        }
    }
}
