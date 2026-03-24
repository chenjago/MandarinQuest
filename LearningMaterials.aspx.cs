using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class LearningMaterials : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMaterials("");
            }
        }

        void LoadMaterials(string keyword)
        {
            SqlDataAdapter da;

            if (keyword == "")
            {
                da = new SqlDataAdapter(
                @"SELECT FileName, FilePath, MaterialType
                  FROM LearningMaterials
                  WHERE LevelID IS NULL",
                con);
            }
            else
            {
                da = new SqlDataAdapter(
                @"SELECT FileName, FilePath, MaterialType
                  FROM LearningMaterials
                  WHERE LevelID IS NULL
                  AND FileName LIKE @k",
                con);

                da.SelectCommand.Parameters.AddWithValue("@k", "%" + keyword + "%");
            }

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptMaterials.DataSource = dt;
            rptMaterials.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMaterials(txtSearch.Text.Trim());
        }

        protected void ViewMaterial(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string file = e.CommandArgument.ToString();
            Response.Redirect(file);
        }

        protected void DownloadMaterial(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string file = Server.MapPath(e.CommandArgument.ToString());

            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition",
            "attachment; filename=" + System.IO.Path.GetFileName(file));

            Response.TransmitFile(file);
            Response.End();
        }
    }
}