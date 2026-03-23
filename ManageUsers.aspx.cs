using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ManageUsers : System.Web.UI.Page
    {

        SqlConnection con = new SqlConnection(
        @"Data Source=(LocalDB)\MSSQLLocalDB;
        AttachDbFilename=|DataDirectory|\MandarinQuest.mdf;
        Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        void LoadUsers()
        {
            SqlDataAdapter da = new SqlDataAdapter(
            "SELECT UserID, FullName, Email, CreatedDate FROM Users",
            con);

            DataTable dt = new DataTable();

            da.Fill(dt);

            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            string id = gvUsers.DataKeys[e.RowIndex].Value.ToString();

            GridViewRow row = gvUsers.Rows[e.RowIndex];

            string name = ((TextBox)row.Cells[1].Controls[0]).Text;
            string email = ((TextBox)row.Cells[2].Controls[0]).Text;

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "UPDATE Users SET FullName=@name, Email=@email WHERE UserID=@id", con);

            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            con.Close();

            gvUsers.EditIndex = -1;

            LoadUsers();
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            string id = gvUsers.DataKeys[e.RowIndex].Value.ToString();

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "DELETE FROM Users WHERE UserID=@id", con);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            con.Close();

            LoadUsers();
        }

    }
}