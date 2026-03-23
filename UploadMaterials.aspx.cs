using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MandarinQuest
{
    public partial class UploadMaterials : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Role"] == null || Session["Role"].ToString() != "teacher")
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadLessons();
                LoadMaterials();
            }

        }

        void LoadLessons()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LessonID,LessonTitle FROM Lessons", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlLessons.DataSource = dt;
            ddlLessons.DataTextField = "LessonTitle";
            ddlLessons.DataValueField = "LessonID";
            ddlLessons.DataBind();

            ddlLessons.Items.Insert(0, new System.Web.UI.WebControls.ListItem("General Resource (No Lesson)", "0"));

        }

        void LoadMaterials()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT MaterialID,FileName,FilePath,UploadDate FROM LearningMaterials", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvMaterials.DataSource = dt;
            gvMaterials.DataBind();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (!fileUpload.HasFile)
                return;

            string folder = Server.MapPath("~/Materials/");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = Path.GetFileName(fileUpload.FileName);

            string path = folder + fileName;

            fileUpload.SaveAs(path);

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "INSERT INTO LearningMaterials (FileName,FilePath,UploadDate,UploadedBy) VALUES (@n,@p,GETDATE(),@u)", con);

            cmd.Parameters.AddWithValue("@n", txtMaterialName.Text);
            cmd.Parameters.AddWithValue("@p", "Materials/" + fileName);
            cmd.Parameters.AddWithValue("@u", Session["UserID"]);

            cmd.ExecuteNonQuery();

            SqlCommand getID = new SqlCommand("SELECT MAX(MaterialID) FROM LearningMaterials", con);

            int materialID = Convert.ToInt32(getID.ExecuteScalar());

            if (ddlLessons.SelectedValue != "0")
            {

                SqlCommand link = new SqlCommand(
                "INSERT INTO LessonMaterials (LessonID,MaterialID) VALUES (@l,@m)", con);

                link.Parameters.AddWithValue("@l", ddlLessons.SelectedValue);
                link.Parameters.AddWithValue("@m", materialID);

                link.ExecuteNonQuery();

            }

            con.Close();

            LoadMaterials();

        }

        protected void gvMaterials_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            if (e.CommandName == "deleteMaterial")
            {

                int materialID = Convert.ToInt32(e.CommandArgument);

                con.Open();

                // Get file path first
                SqlCommand getFile = new SqlCommand(
                "SELECT FilePath FROM LearningMaterials WHERE MaterialID=@id", con);

                getFile.Parameters.AddWithValue("@id", materialID);

                string filePath = getFile.ExecuteScalar().ToString();

                // Delete file from server
                string fullPath = Server.MapPath("~/" + filePath);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                // Remove lesson links
                SqlCommand deleteLink = new SqlCommand(
                "DELETE FROM LessonMaterials WHERE MaterialID=@id", con);

                deleteLink.Parameters.AddWithValue("@id", materialID);
                deleteLink.ExecuteNonQuery();

                // Delete material record
                SqlCommand deleteMaterial = new SqlCommand(
                "DELETE FROM LearningMaterials WHERE MaterialID=@id", con);

                deleteMaterial.Parameters.AddWithValue("@id", materialID);
                deleteMaterial.ExecuteNonQuery();

                con.Close();

                LoadMaterials();

            }

        }

    }
}