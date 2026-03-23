using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace MandarinQuest
{
    public partial class ManageLessons : System.Web.UI.Page
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
                LoadClasses();

        }

        void LoadClasses()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LevelID,LevelName FROM Levels WHERE Status='Active'", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlClasses.DataSource = dt;
            ddlClasses.DataTextField = "LevelName";
            ddlClasses.DataValueField = "LevelID";
            ddlClasses.DataBind();

            LoadLessons();

        }

        void LoadLessons()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT L.LessonID,
                 V.LevelName,
                 L.LessonTitle,
                 L.Description,
                 L.CreatedDate
          FROM Lessons L
          JOIN Levels V ON L.LevelID = V.LevelID
          WHERE L.LevelID=@c", con);

            da.SelectCommand.Parameters.AddWithValue("@c", ddlClasses.SelectedValue);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvLessons.DataSource = dt;
            gvLessons.DataBind();

        }

        protected void ddlClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLessons();
        }

        protected void btnAddLesson_Click(object sender, EventArgs e)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "INSERT INTO Lessons (LevelID,LessonTitle,Description,CreatedDate) VALUES (@c,@t,@d,GETDATE())",
            con);

            cmd.Parameters.AddWithValue("@c", ddlClasses.SelectedValue);
            cmd.Parameters.AddWithValue("@t", txtLessonTitle.Text);
            cmd.Parameters.AddWithValue("@d", txtDescription.Text);

            cmd.ExecuteNonQuery();

            con.Close();

            LoadLessons();

        }

        protected void gvLessons_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {

            if (e.CommandName == "DeleteLesson")
            {

                con.Open();

                SqlCommand cmd = new SqlCommand(
                "DELETE FROM Lessons WHERE LessonID=@id", con);

                cmd.Parameters.AddWithValue("@id", e.CommandArgument);

                cmd.ExecuteNonQuery();

                con.Close();

                LoadLessons();

            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherPage.aspx");
        }

    }


}
