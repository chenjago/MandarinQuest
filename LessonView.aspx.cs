using System;
using System.Data;
using System.Data.SqlClient;

namespace MandarinQuest
{
    public partial class LessonView : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True;");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadLessons();
            }
        }

        void LoadLessons()
        {
            int levelID = Convert.ToInt32(Request.QueryString["level"]);

            SqlDataAdapter da = new SqlDataAdapter(
            @"SELECT L.LessonID, L.LessonTitle,
              CASE 
                 WHEN SP.LessonID IS NULL THEN 0
                 ELSE 1
              END AS Completed
              FROM Lessons L
              LEFT JOIN StudentProgress SP
              ON L.LessonID = SP.LessonID
              AND SP.UserID = @uid
              AND SP.Status='Completed'
              WHERE L.LevelID = @level",
              con);

            da.SelectCommand.Parameters.AddWithValue("@uid", Session["UserID"]);
            da.SelectCommand.Parameters.AddWithValue("@level", levelID);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptLessons.DataSource = dt;
            rptLessons.DataBind();
        }

        protected void OpenLesson(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            int lessonID = Convert.ToInt32(e.CommandArgument);

            Response.Redirect("LessonMaterials.aspx?LessonID=" + lessonID);
        }

        protected void CompleteLesson(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {

            int lessonID = Convert.ToInt32(e.CommandArgument);
            int userID = Convert.ToInt32(Session["UserID"]);

            con.Open();

            SqlCommand check = new SqlCommand(
            "SELECT COUNT(*) FROM StudentProgress WHERE UserID=@u AND LessonID=@l",
            con);

            check.Parameters.AddWithValue("@u", userID);
            check.Parameters.AddWithValue("@l", lessonID);

            int count = Convert.ToInt32(check.ExecuteScalar());

            if (count == 0)
            {

                SqlCommand cmd = new SqlCommand(
                "INSERT INTO StudentProgress(UserID,LessonID,Status,CompletionDate) VALUES(@u,@l,'Completed',GETDATE())",
                con);

                cmd.Parameters.AddWithValue("@u", userID);
                cmd.Parameters.AddWithValue("@l", lessonID);

                cmd.ExecuteNonQuery();

                Response.Write("<script>alert('Lesson completed successfully!');</script>");

            }
            else
            {

                Response.Write("<script>alert('Lesson already completed.');</script>");

            }

            con.Close();

            LoadLessons();
        }

    }
}