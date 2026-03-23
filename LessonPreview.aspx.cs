using System;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class LessonPreview : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPreviewLesson();
            }
        }

        void LoadPreviewLesson()
        {
            string q = "SELECT TOP 1 LessonTitle, ContentPath FROM Lessons";

            SqlCommand cmd = new SqlCommand(q, con);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lblLessonTitle.Text = dr["LessonTitle"].ToString();
                txtPreviewContent.Text = dr["LessonContent"].ToString();
            }
            else
            {
                lblLessonTitle.Text = "No Lesson Available";
                txtPreviewContent.Text = "Lesson content will appear here.";
            }

            dr.Close();
            con.Close();
        }


    }
}
