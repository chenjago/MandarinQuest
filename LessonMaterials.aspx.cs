using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class LessonMaterials : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadMaterials();
            }

        }

        void LoadMaterials()
        {

            int lessonID = Convert.ToInt32(Request.QueryString["LessonID"]);

            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT LM.FileName, LM.FilePath, LM.MaterialType
              FROM LessonMaterials LSM
              JOIN LearningMaterials LM ON LSM.MaterialID = LM.MaterialID
              WHERE LSM.LessonID = @lesson",
              con);

            da.SelectCommand.Parameters.AddWithValue("@lesson", lessonID);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptMaterials.DataSource = dt;
            rptMaterials.DataBind();

            if (dt.Rows.Count == 0)
            {
                lblNoMaterial.Visible = true;
            }

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

        protected void btnBack_Click(object sender, EventArgs e)
        {

            Response.Redirect("StudentPage.aspx");

        }

    }
}