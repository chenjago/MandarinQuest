using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class AuditLogs : System.Web.UI.Page
    {
        string connStr = @"Data Source=(LocalDB)\MSSQLLocalDB;
                           AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
                           Integrated Security=True;Connect Timeout=30";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLogs();
            }
        }

        private void LoadLogs()
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();

                    string query = @"SELECT UserID, Action, Description, LogDate
                                     FROM AuditLogs 
                                     ORDER BY LogDate DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvAuditLogs.DataSource = dt;
                    gvAuditLogs.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("Error: " + ex.Message);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPage.aspx");
        }

        protected void gvAuditLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAuditLogs.PageIndex = e.NewPageIndex;
            LoadLogs();
        }

        protected void gvAuditLogs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string action = e.Row.Cells[1].Text;

                if (action == "Delete" || action == "PasswordReset")
                {
                    e.Row.BackColor = System.Drawing.Color.FromArgb(255, 230, 230); // light red
                }
            }
        }
    }
}