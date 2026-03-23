using System;
using System.Data;

namespace MandarinQuest
{
    public partial class ManageRoles : System.Web.UI.Page
    {

        DataTable roles;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateRoles();
                BindGrid();
            }
        }

        void CreateRoles()
        {

            roles = new DataTable();

            roles.Columns.Add("RoleID");
            roles.Columns.Add("RoleName");

            roles.Rows.Add("1", "Admin");
            roles.Rows.Add("2", "Teacher");
            roles.Rows.Add("3", "Student");

            ViewState["Roles"] = roles;
        }

        void BindGrid()
        {
            gvRoles.DataSource = (DataTable)ViewState["Roles"];
            gvRoles.DataBind();
        }

        protected void btnAddRole_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)ViewState["Roles"];

            string id = (dt.Rows.Count + 1).ToString();
            string role = txtRole.Text;

            dt.Rows.Add(id, role);

            ViewState["Roles"] = dt;

            txtRole.Text = "";

            BindGrid();
        }

        protected void gvRoles_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvRoles.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvRoles_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvRoles.EditIndex = -1;
            BindGrid();
        }

        protected void gvRoles_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["Roles"];

            string id = gvRoles.DataKeys[e.RowIndex].Value.ToString();

            string role = ((System.Web.UI.WebControls.TextBox)
                gvRoles.Rows[e.RowIndex].Cells[1].Controls[0]).Text;

            foreach (DataRow row in dt.Rows)
            {
                if (row["RoleID"].ToString() == id)
                {
                    row["RoleName"] = role;
                }
            }

            gvRoles.EditIndex = -1;

            ViewState["Roles"] = dt;

            BindGrid();
        }

        protected void gvRoles_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {

            DataTable dt = (DataTable)ViewState["Roles"];

            string id = gvRoles.DataKeys[e.RowIndex].Value.ToString();

            foreach (DataRow row in dt.Rows)
            {
                if (row["RoleID"].ToString() == id)
                {
                    row.Delete();
                    break;
                }
            }

            ViewState["Roles"] = dt;

            BindGrid();
        }

    }
}