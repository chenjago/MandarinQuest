using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MandarinQuest
{
    public partial class ClassSessions : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["db"].ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSessions();
            }
        }

        void LoadSessions()
        {
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT SessionID, SessionTitle, SessionDate, SessionLink FROM ClassSessions",
            con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            gvSessions.DataSource = dt;
            gvSessions.DataBind();
        }

    }
}