using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ScheduleSessions : System.Web.UI.Page
    {


    SqlConnection con = new SqlConnection(
    @"Data Source=(LocalDB)\MSSQLLocalDB;
    AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
    Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {

            // Only teacher can access
            if (Session["Role"] == null || Session["Role"].ToString() != "teacher")
                Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadClasses();
                LoadSessions();
            }

        }

        // Load Levels into dropdown
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

        }

        // Load scheduled sessions
        void LoadSessions()
        {

            SqlDataAdapter da = new SqlDataAdapter(

            @"SELECT 
            CS.SessionID,
            L.LevelName,
            CS.SessionTitle,
            CS.SessionDate,
            CS.SessionLink
          FROM ClassSessions CS
          JOIN Levels L ON CS.LevelID = L.LevelID
          ORDER BY CS.SessionDate DESC", con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvSessions.DataSource = dt;
            gvSessions.DataBind();

        }

        // Create new session
        protected void btnCreateSession_Click(object sender, EventArgs e)
        {

            if (txtTitle.Text.Trim() == "" || txtDate.Text == "" || txtTime.Text == "")
                return;

            DateTime sessionDateTime =
            Convert.ToDateTime(txtDate.Text + " " + txtTime.Text);

            con.Open();

            SqlCommand cmd = new SqlCommand(
            @"INSERT INTO ClassSessions
          (LevelID,SessionTitle,SessionDate,SessionLink,CreatedBy)
          VALUES(@l,@t,@d,@link,@u)", con);

            cmd.Parameters.AddWithValue("@l", ddlClasses.SelectedValue);
            cmd.Parameters.AddWithValue("@t", txtTitle.Text);
            cmd.Parameters.AddWithValue("@d", sessionDateTime);
            cmd.Parameters.AddWithValue("@link", txtLink.Text);
            cmd.Parameters.AddWithValue("@u", Session["UserID"]);

            cmd.ExecuteNonQuery();

            con.Close();

            // Clear fields
            txtTitle.Text = "";
            txtDate.Text = "";
            txtTime.Text = "";
            txtLink.Text = "";

            LoadSessions();

        }

        // Delete session
        protected void gvSessions_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "DeleteSession")
            {

                con.Open();

                SqlCommand cmd = new SqlCommand(
                "DELETE FROM ClassSessions WHERE SessionID=@id", con);

                cmd.Parameters.AddWithValue("@id", e.CommandArgument);

                cmd.ExecuteNonQuery();

                con.Close();

                LoadSessions();

            }

        }

    }


}
