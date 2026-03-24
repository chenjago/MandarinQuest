using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class ViewParticipation : System.Web.UI.Page
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null || Session["Role"].ToString() != "teacher")
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadLevels();
                LoadLessons();
                LoadParticipation();
            }
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private void LoadLevels()
        {
            using (SqlConnection con = CreateConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT LevelID, LevelName
                  FROM Levels
                  WHERE Status = 'Active'
                  ORDER BY LevelOrder ASC, LevelName ASC", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLevels.DataSource = dt;
                ddlLevels.DataTextField = "LevelName";
                ddlLevels.DataValueField = "LevelID";
                ddlLevels.DataBind();

                ddlLevels.Items.Insert(0, new ListItem("All Levels", ""));
            }
        }

        private void LoadLessons()
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;

                string query = @"
                    SELECT L.LessonID, L.LessonTitle
                    FROM Lessons L
                    INNER JOIN Levels LV ON L.LevelID = LV.LevelID
                    WHERE LV.Status = 'Active'";

                if (!string.IsNullOrWhiteSpace(ddlLevels.SelectedValue))
                {
                    query += " AND L.LevelID = @LevelID";
                    cmd.Parameters.AddWithValue("@LevelID", ddlLevels.SelectedValue);
                }

                query += " ORDER BY L.LessonTitle ASC";
                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlLessons.DataSource = dt;
                ddlLessons.DataTextField = "LessonTitle";
                ddlLessons.DataValueField = "LessonID";
                ddlLessons.DataBind();

                ddlLessons.Items.Insert(0, new ListItem("All Lessons", ""));
            }
        }

        private void LoadParticipation()
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;

                string query = @"
                    SELECT 
                        SP.UserID,
                        U.FullName,
                        LV.LevelName,
                        L.LessonTitle,
                        SP.Status AS CompletionStatus,
                        SP.CompletionDate
                    FROM StudentProgress SP
                    INNER JOIN Users U ON SP.UserID = U.UserID
                    INNER JOIN Lessons L ON SP.LessonID = L.LessonID
                    INNER JOIN Levels LV ON L.LevelID = LV.LevelID
                    WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(ddlLevels.SelectedValue))
                {
                    query += " AND L.LevelID = @LevelID";
                    cmd.Parameters.AddWithValue("@LevelID", ddlLevels.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(ddlLessons.SelectedValue))
                {
                    query += " AND L.LessonID = @LessonID";
                    cmd.Parameters.AddWithValue("@LessonID", ddlLessons.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(ddlStatus.SelectedValue))
                {
                    query += " AND SP.Status = @Status";
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                }

                if (!string.IsNullOrWhiteSpace(txtSearchStudent.Text))
                {
                    query += " AND U.FullName LIKE @StudentName";
                    cmd.Parameters.AddWithValue("@StudentName", "%" + txtSearchStudent.Text.Trim() + "%");
                }

                query += " ORDER BY SP.CompletionDate DESC, U.FullName ASC";

                cmd.CommandText = query;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvParticipation.DataSource = dt;
                dgvParticipation.DataBind();

                UpdateSummaryCards(dt);
            }
        }

        private void UpdateSummaryCards(DataTable dt)
        {
            int totalRecords = dt.Rows.Count;
            int completedRecords = 0;
            HashSet<string> uniqueStudents = new HashSet<string>();

            foreach (DataRow row in dt.Rows)
            {
                string userId = Convert.ToString(row["UserID"]);
                string status = Convert.ToString(row["CompletionStatus"]);

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    uniqueStudents.Add(userId);
                }

                if (string.Equals(status, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    completedRecords++;
                }
            }

            decimal completionRate = totalRecords > 0
                ? Math.Round((decimal)completedRecords * 100m / totalRecords, 1)
                : 0m;

            lblTotalRecords.Text = totalRecords.ToString();
            lblTotalStudents.Text = uniqueStudents.Count.ToString();
            lblCompletedRecords.Text = completedRecords.ToString();
            lblCompletionRate.Text = completionRate.ToString("0.#") + "%";
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

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadParticipation();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadParticipation();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlLevels.SelectedIndex = 0;
            LoadLessons();
            ddlLessons.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            txtSearchStudent.Text = string.Empty;
            LoadParticipation();
        }

        protected void dgvParticipation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            string status = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CompletionStatus"));
            object completedOnValue = DataBinder.Eval(e.Row.DataItem, "CompletionDate");

            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblCompletedOn = (Label)e.Row.FindControl("lblCompletedOn");

            if (lblStatus != null)
            {
                lblStatus.CssClass = "status-badge " + GetStatusCss(status);
                lblStatus.Text = string.IsNullOrWhiteSpace(status) ? "Unknown" : status;
            }

            if (lblCompletedOn != null)
            {
                if (completedOnValue == DBNull.Value || completedOnValue == null)
                {
                    lblCompletedOn.Text = "-";
                }
                else
                {
                    DateTime completedOn;
                    if (DateTime.TryParse(completedOnValue.ToString(), out completedOn))
                    {
                        lblCompletedOn.Text = completedOn.ToString("dd/MM/yyyy hh:mm tt");
                    }
                    else
                    {
                        lblCompletedOn.Text = completedOnValue.ToString();
                    }
                }
            }
        }

        private string GetStatusCss(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "status-other";

            switch (status.Trim().ToLower())
            {
                case "completed":
                    return "status-completed";
                case "in progress":
                    return "status-inprogress";
                case "not started":
                    return "status-notstarted";
                default:
                    return "status-other";
            }
        }
    }
}