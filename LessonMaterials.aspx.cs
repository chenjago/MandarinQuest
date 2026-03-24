using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

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
                return;
            }

            if (!IsPostBack)
            {
                int lessonID;
                if (!int.TryParse(Request.QueryString["LessonID"], out lessonID))
                {
                    Response.Redirect("StudentPage.aspx");
                    return;
                }

                LoadMaterials(lessonID);
                LoadQuizStatus(lessonID);
            }
        }

        void LoadMaterials(int lessonID)
        {
            SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT LM.FileName, LM.FilePath, LM.MaterialType
                  FROM LessonMaterials LSM
                  INNER JOIN LearningMaterials LM ON LSM.MaterialID = LM.MaterialID
                  WHERE LSM.LessonID = @lessonID",
                con);

            da.SelectCommand.Parameters.AddWithValue("@lessonID", lessonID);

            DataTable dt = new DataTable();
            da.Fill(dt);

            rptMaterials.DataSource = dt;
            rptMaterials.DataBind();

            lblNoMaterial.Visible = (dt.Rows.Count == 0);
        }

        void LoadQuizStatus(int lessonID)
        {
            int userID = Convert.ToInt32(Session["UserID"]);

            SqlCommand cmd = new SqlCommand(
                @"SELECT TOP 1 Q.QuizID, Q.QuizTitle, QA.Status
                  FROM Quiz Q
                  LEFT JOIN QuizAttempts QA 
                      ON Q.QuizID = QA.QuizID AND QA.UserID = @userID
                  WHERE Q.LessonID = @lessonID AND Q.QuizStatus = 'Published'
                  ORDER BY QA.AttemptDate DESC",
                con);

            cmd.Parameters.AddWithValue("@lessonID", lessonID);
            cmd.Parameters.AddWithValue("@userID", userID);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                pnlQuiz.Visible = true;

                lblQuizTitle.Text = dr["QuizTitle"].ToString();

                object statusObj = dr["Status"];

                btnTakeQuiz.Visible = false;
                btnPreviewQuiz.Visible = false;
                btnRetakeQuiz.Visible = false;

                if (statusObj == DBNull.Value)
                {
                    btnTakeQuiz.Visible = true;
                    lblQuizStatus.Text = "You have not taken this quiz yet. Passing mark is 4/5.";
                }
                else
                {
                    string status = statusObj.ToString();

                    btnPreviewQuiz.Visible = true;

                    if (status == "Fail")
                    {
                        btnRetakeQuiz.Visible = true;
                        lblQuizStatus.Text = "You failed the quiz. You need to retake it. Passing mark is 4/5.";
                    }
                    else
                    {
                        lblQuizStatus.Text = "You passed the quiz.";
                    }
                }
            }
            else
            {
                pnlQuiz.Visible = false;
            }

            dr.Close();
            con.Close();
        }

        protected void ViewMaterial(object sender, CommandEventArgs e)
        {
            string filePath = e.CommandArgument.ToString();
            Response.Redirect(filePath);
        }

        protected void DownloadMaterial(object sender, CommandEventArgs e)
        {
            string relativePath = e.CommandArgument.ToString();
            string physicalPath = Server.MapPath(relativePath);

            if (File.Exists(physicalPath))
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition",
                    "attachment; filename=" + Path.GetFileName(physicalPath));
                Response.TransmitFile(physicalPath);
                Response.End();
            }
            else
            {
                lblMessage.Text = "File not found.";
            }
        }

        protected void btnTakeQuiz_Click(object sender, EventArgs e)
        {
            int lessonID;
            if (int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("TakeQuiz.aspx?LessonID=" + lessonID);
            }
        }

        protected void btnPreviewQuiz_Click(object sender, EventArgs e)
        {
            int lessonID;
            if (int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("QuizReview.aspx?LessonID=" + lessonID);
            }
        }

        protected void btnRetakeQuiz_Click(object sender, EventArgs e)
        {
            int lessonID;
            if (int.TryParse(Request.QueryString["LessonID"], out lessonID))
            {
                Response.Redirect("TakeQuiz.aspx?LessonID=" + lessonID + "&retake=1");
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentPage.aspx");
        }
    }
}