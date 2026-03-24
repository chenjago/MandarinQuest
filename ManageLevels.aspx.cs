using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace MandarinQuest
{
    public partial class ManageLevels : System.Web.UI.Page
    {

    SqlConnection con = new SqlConnection(
    @"Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
    Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || (Session["Role"].ToString() != "teacher" && Session["Role"].ToString() != "admin"))
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadLevels();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("TeacherPage.aspx");
        }

        void LoadLevels()
        {

            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT LevelID,LevelName,Description,LevelOrder,Status FROM Levels WHERE CreatedBy=@t AND Status='Active' ORDER BY LevelOrder",
            con);

            da.SelectCommand.Parameters.AddWithValue("@t", Session["UserID"]);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvLevels.DataSource = dt;
            dgvLevels.DataBind();

        }

        protected void btnAddLevel_Click(object sender, EventArgs e)
        {


        con.Open();

            // Get next LevelOrder automatically
            SqlCommand orderCmd = new SqlCommand(
            "SELECT ISNULL(MAX(LevelOrder),0) + 1 FROM Levels", con);

            int nextOrder = Convert.ToInt32(orderCmd.ExecuteScalar());

            SqlCommand cmd = new SqlCommand(
            "INSERT INTO Levels (LevelName,Description,LevelOrder,CreatedBy,Status) VALUES (@n,@d,@o,@t,'Active')",
            con);

            cmd.Parameters.AddWithValue("@n", txtLevelName.Text);
            cmd.Parameters.AddWithValue("@d", txtDescription.Text);
            cmd.Parameters.AddWithValue("@o", nextOrder);
            cmd.Parameters.AddWithValue("@t", Session["UserID"]);

            cmd.ExecuteNonQuery();

            con.Close();

            txtLevelName.Text = "";
            txtDescription.Text = "";

            LoadLevels();

            ScriptManager.RegisterStartupScript(this, GetType(), "toast",
            "showToast('Level added successfully');", true);

        }


        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Levels SET Status='Deleted',DeleteReason=@r WHERE LevelID=@id",
            con);

            cmd.Parameters.AddWithValue("@r", txtDeleteReason.Text);
            cmd.Parameters.AddWithValue("@id", hiddenLevelID.Value);

            cmd.ExecuteNonQuery();

            con.Close();

            LoadLevels();

            ScriptManager.RegisterStartupScript(this, GetType(), "toast",
            "showToast('Level deleted successfully');", true);

        }

    }


}
