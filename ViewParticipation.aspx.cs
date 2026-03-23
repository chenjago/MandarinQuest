using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ViewParticipation : System.Web.UI.Page
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
                LoadLevels();
                LoadParticipation();
            }

        }


        void LoadLevels()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LevelID,LevelName FROM Levels WHERE Status='Active'", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlLevels.DataSource = dt;
            ddlLevels.DataTextField = "LevelName";
            ddlLevels.DataValueField = "LevelID";
            ddlLevels.DataBind();

            ddlLevels.Items.Insert(0, new ListItem("All Levels", "0"));

            LoadLessons();

        }


        void LoadLessons()
        {

            SqlDataAdapter da;

            if (ddlLevels.SelectedValue == "0")
            {
                da = new SqlDataAdapter(
                "SELECT LessonID,LessonTitle FROM Lessons", con);
            }
            else
            {
                da = new SqlDataAdapter(
                "SELECT LessonID,LessonTitle FROM Lessons WHERE LevelID=@l", con);

                da.SelectCommand.Parameters.AddWithValue("@l", ddlLevels.SelectedValue);
            }

            DataTable dt = new DataTable();
            da.Fill(dt);

            ddlLessons.DataSource = dt;
            ddlLessons.DataTextField = "LessonTitle";
            ddlLessons.DataValueField = "LessonID";
            ddlLessons.DataBind();

            ddlLessons.Items.Insert(0, new ListItem("All Lessons", "0"));

        }


        void LoadParticipation()
        {

            string query = @"
                SELECT 
                U.FullName,
                LV.LevelName,
                L.LessonTitle,
                SP.Status AS CompletionStatus,
                SP.CompletionDate
                FROM StudentProgress SP
                JOIN Users U ON SP.UserID = U.UserID
                JOIN Lessons L ON SP.LessonID = L.LessonID
                JOIN Levels LV ON L.LevelID = LV.LevelID
                WHERE 1=1
                ";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            if (ddlLevels.SelectedValue != "0")
            {
                query += " AND L.LevelID=@level";
                cmd.Parameters.AddWithValue("@level", ddlLevels.SelectedValue);
            }

            if (ddlLessons.SelectedValue != "0")
            {
                query += " AND LS.LessonID=@lesson";
                cmd.Parameters.AddWithValue("@lesson", ddlLessons.SelectedValue);
            }

            cmd.CommandText = query;

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvParticipation.DataSource = dt;
            dgvParticipation.DataBind();

        }


        protected void ddlLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadLessons();
            LoadParticipation();
        }


        protected void ddlLessons_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadParticipation();
        }

    }


}
