using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace MandarinQuest
{
    public partial class UploadMaterials : System.Web.UI.Page
    {
        private readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["MandarinQuestDB"].ConnectionString;

        private const string GeneralResourceValue = "GR";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["Role"] == null || (Session["Role"].ToString() != "teacher" && Session["Role"].ToString() != "admin"))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                InitializePage();
            }
        }

        private void InitializePage()
        {
            LoadUploadLevels(null, null, null);
            LoadUploadLessons(string.Empty, null);

            LoadFilterLevels();
            LoadFilterLessons(string.Empty);

            ClearFormFields();
            LoadMaterials();
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Visible = true;
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "message success" : "message error";
        }

        private void LoadUploadLevels(string selectedValue, int? includeLevelId, string includeLevelName)
        {
            ddlLevelsUpload.Items.Clear();
            ddlLevelsUpload.Items.Add(new ListItem("-- Select Level --", string.Empty));
            ddlLevelsUpload.Items.Add(new ListItem("GR (General Resources)", GeneralResourceValue));

            using (SqlConnection con = CreateConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT LevelID, LevelName
                  FROM Levels
                  WHERE Status = 'Active'
                  ORDER BY LevelOrder ASC, LevelName ASC", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ddlLevelsUpload.Items.Add(new ListItem(
                        Convert.ToString(row["LevelName"]),
                        Convert.ToString(row["LevelID"])));
                }
            }

            if (includeLevelId.HasValue && includeLevelId.Value > 0 &&
                ddlLevelsUpload.Items.FindByValue(includeLevelId.Value.ToString()) == null)
            {
                string fallbackName = string.IsNullOrWhiteSpace(includeLevelName)
                    ? "[Inactive / Deleted Level]"
                    : includeLevelName;

                ddlLevelsUpload.Items.Add(new ListItem(fallbackName, includeLevelId.Value.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(selectedValue) &&
                ddlLevelsUpload.Items.FindByValue(selectedValue) != null)
            {
                ddlLevelsUpload.SelectedValue = selectedValue;
            }
            else
            {
                ddlLevelsUpload.SelectedIndex = 0;
            }
        }

        private void LoadFilterLevels()
        {
            ddlFilterLevel.Items.Clear();
            ddlFilterLevel.Items.Add(new ListItem("All Levels / GR", string.Empty));
            ddlFilterLevel.Items.Add(new ListItem("GR", GeneralResourceValue));

            using (SqlConnection con = CreateConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT LevelID, LevelName
                  FROM Levels
                  WHERE Status = 'Active'
                  ORDER BY LevelOrder ASC, LevelName ASC", con))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ddlFilterLevel.Items.Add(new ListItem(
                        Convert.ToString(row["LevelName"]),
                        Convert.ToString(row["LevelID"])));
                }
            }
        }

        private void LoadUploadLessons(string selectedLevelValue, string selectedLessonValue)
        {
            ddlLessonsUpload.Items.Clear();

            if (string.IsNullOrWhiteSpace(selectedLevelValue))
            {
                ddlLessonsUpload.Items.Add(new ListItem("-- Select Level First --", string.Empty));
                ddlLessonsUpload.Enabled = false;
                return;
            }

            if (selectedLevelValue == GeneralResourceValue)
            {
                ddlLessonsUpload.Items.Add(new ListItem("Not required for GR", string.Empty));
                ddlLessonsUpload.Enabled = false;
                return;
            }

            int levelId;
            if (!int.TryParse(selectedLevelValue, out levelId) || levelId <= 0)
            {
                ddlLessonsUpload.Items.Add(new ListItem("-- Select Level First --", string.Empty));
                ddlLessonsUpload.Enabled = false;
                return;
            }

            ddlLessonsUpload.Enabled = true;
            ddlLessonsUpload.Items.Add(new ListItem("-- Select Lesson --", string.Empty));

            using (SqlConnection con = CreateConnection())
            using (SqlDataAdapter da = new SqlDataAdapter(
                @"SELECT LessonID, LessonTitle
                  FROM Lessons
                  WHERE LevelID = @LevelID
                  ORDER BY LessonTitle ASC", con))
            {
                da.SelectCommand.Parameters.AddWithValue("@LevelID", levelId);

                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ddlLessonsUpload.Items.Add(new ListItem(
                        Convert.ToString(row["LessonTitle"]),
                        Convert.ToString(row["LessonID"])));
                }
            }

            if (!string.IsNullOrWhiteSpace(selectedLessonValue) &&
                ddlLessonsUpload.Items.FindByValue(selectedLessonValue) != null)
            {
                ddlLessonsUpload.SelectedValue = selectedLessonValue;
            }
        }

        private void LoadFilterLessons(string selectedLevelValue)
        {
            ddlFilterLesson.Items.Clear();

            if (selectedLevelValue == GeneralResourceValue)
            {
                ddlFilterLesson.Items.Add(new ListItem("Not applicable for GR", string.Empty));
                ddlFilterLesson.Enabled = false;
                return;
            }

            ddlFilterLesson.Enabled = true;
            ddlFilterLesson.Items.Add(new ListItem("All Lessons", string.Empty));

            string sql = @"
SELECT ls.LessonID, ls.LessonTitle
FROM Lessons ls
INNER JOIN Levels lv ON ls.LevelID = lv.LevelID
WHERE lv.Status = 'Active'";

            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;

                if (!string.IsNullOrWhiteSpace(selectedLevelValue))
                {
                    int levelId;
                    if (int.TryParse(selectedLevelValue, out levelId) && levelId > 0)
                    {
                        sql += " AND ls.LevelID = @LevelID";
                        cmd.Parameters.AddWithValue("@LevelID", levelId);
                    }
                }

                sql += " ORDER BY ls.LessonTitle ASC";
                cmd.CommandText = sql;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        ddlFilterLesson.Items.Add(new ListItem(
                            Convert.ToString(row["LessonTitle"]),
                            Convert.ToString(row["LessonID"])));
                    }
                }
            }
        }

        private void LoadMaterials()
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = con;

                cmd.CommandText = @"
WITH MaterialBase AS
(
    SELECT
        lm.MaterialID,
        lm.FileName,
        lm.FilePath,
        lm.UploadDate,
        lm.LevelID AS StoredLevelID,
        linkMap.LessonID,
        ls.LessonTitle,
        ls.LevelID AS LessonLevelID
    FROM LearningMaterials lm
    OUTER APPLY
    (
        SELECT TOP 1 LessonID
        FROM LessonMaterials
        WHERE MaterialID = lm.MaterialID
        ORDER BY LessonID
    ) linkMap
    LEFT JOIN Lessons ls ON linkMap.LessonID = ls.LessonID
),
MaterialView AS
(
    SELECT
        mb.MaterialID,
        mb.FileName,
        mb.FilePath,
        mb.UploadDate,
        mb.LessonID,
        mb.LessonTitle,
        COALESCE(mb.StoredLevelID, mb.LessonLevelID) AS EffectiveLevelID,
        CASE
            WHEN mb.StoredLevelID IS NULL AND mb.LessonID IS NULL THEN 1
            ELSE 0
        END AS IsGeneralResource
    FROM MaterialBase mb
)
SELECT
    mv.MaterialID,
    CASE
        WHEN mv.IsGeneralResource = 1 THEN 'GR'
        WHEN mv.EffectiveLevelID IS NOT NULL AND lv.LevelID IS NULL THEN '[Deleted Level]'
        WHEN lv.LevelName IS NULL THEN '-'
        ELSE lv.LevelName
    END AS LevelName,
    CASE
        WHEN mv.IsGeneralResource = 1 THEN 'General Resource'
        WHEN mv.LessonID IS NULL THEN '-'
        ELSE mv.LessonTitle
    END AS LessonTitle,
    mv.FileName,
    mv.UploadDate
FROM MaterialView mv
LEFT JOIN Levels lv ON mv.EffectiveLevelID = lv.LevelID
WHERE 1 = 1";

                string filterLevelValue = ddlFilterLevel.SelectedValue;

                if (filterLevelValue == GeneralResourceValue)
                {
                    cmd.CommandText += " AND mv.IsGeneralResource = 1";
                }
                else
                {
                    int filterLevelId;
                    if (int.TryParse(filterLevelValue, out filterLevelId) && filterLevelId > 0)
                    {
                        cmd.CommandText += " AND mv.EffectiveLevelID = @FilterLevelID";
                        cmd.Parameters.AddWithValue("@FilterLevelID", filterLevelId);
                    }
                }

                if (ddlFilterLesson.Enabled && !string.IsNullOrWhiteSpace(ddlFilterLesson.SelectedValue))
                {
                    int filterLessonId;
                    if (int.TryParse(ddlFilterLesson.SelectedValue, out filterLessonId) && filterLessonId > 0)
                    {
                        cmd.CommandText += " AND mv.LessonID = @FilterLessonID";
                        cmd.Parameters.AddWithValue("@FilterLessonID", filterLessonId);
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                {
                    cmd.CommandText += " AND mv.FileName LIKE @Search";
                    cmd.Parameters.AddWithValue("@Search", "%" + txtSearch.Text.Trim() + "%");
                }

                cmd.CommandText += " ORDER BY mv.UploadDate DESC, mv.MaterialID DESC";

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvMaterials.DataSource = dt;
                    gvMaterials.DataBind();
                }
            }
        }

        protected void ddlLevelsUpload_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUploadLessons(ddlLevelsUpload.SelectedValue, null);
        }

        protected void ddlFilterLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFilterLessons(ddlFilterLevel.SelectedValue);
            LoadMaterials();
        }

        protected void ddlFilterLesson_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMaterials();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadMaterials();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadFilterLevels();
            LoadFilterLessons(string.Empty);
            LoadMaterials();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string materialName = txtMaterialName.Text.Trim();
            string selectedLevelValue = ddlLevelsUpload.SelectedValue;
            string selectedLessonValue = ddlLessonsUpload.SelectedValue;

            bool isGeneralResource = selectedLevelValue == GeneralResourceValue;

            int? levelId = null;
            int lessonId = 0;

            if (!string.IsNullOrWhiteSpace(selectedLevelValue) && !isGeneralResource)
            {
                int parsedLevelId;
                if (int.TryParse(selectedLevelValue, out parsedLevelId) && parsedLevelId > 0)
                {
                    levelId = parsedLevelId;
                }
            }

            if (!string.IsNullOrWhiteSpace(selectedLessonValue))
            {
                int.TryParse(selectedLessonValue, out lessonId);
            }

            if (string.IsNullOrWhiteSpace(materialName))
            {
                ShowMessage("Please enter a material name.", false);
                return;
            }

            if (string.IsNullOrWhiteSpace(selectedLevelValue))
            {
                ShowMessage("Please select a level or GR.", false);
                return;
            }

            if (!isGeneralResource && (!levelId.HasValue || levelId.Value <= 0))
            {
                ShowMessage("Please select a valid level.", false);
                return;
            }

            if (!isGeneralResource && lessonId <= 0)
            {
                ShowMessage("Please select a lesson.", false);
                return;
            }

            int editMaterialId;
            bool isEdit = int.TryParse(hfEditMaterialID.Value, out editMaterialId) && editMaterialId > 0;

            if (!isEdit && !fileUpload.HasFile)
            {
                ShowMessage("Please choose a file to upload.", false);
                return;
            }

            string folder = Server.MapPath("~/Materials/");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string relativePath = hfOldFilePath.Value;
            string oldRelativePath = hfOldFilePath.Value;
            string newFullPath = string.Empty;

            try
            {
                if (fileUpload.HasFile)
                {
                    string uniqueName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(fileUpload.FileName);
                    newFullPath = Path.Combine(folder, uniqueName);
                    fileUpload.SaveAs(newFullPath);
                    relativePath = "Materials/" + uniqueName;
                }

                using (SqlConnection con = CreateConnection())
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    try
                    {
                        int materialId = editMaterialId;

                        if (isEdit)
                        {
                            using (SqlCommand updateMaterial = new SqlCommand(
                                @"UPDATE LearningMaterials
                                  SET FileName = @FileName,
                                      FilePath = @FilePath,
                                      LevelID = @LevelID
                                  WHERE MaterialID = @MaterialID", con, tran))
                            {
                                updateMaterial.Parameters.AddWithValue("@FileName", materialName);
                                updateMaterial.Parameters.AddWithValue("@FilePath", relativePath);
                                updateMaterial.Parameters.AddWithValue("@LevelID", (object)levelId ?? DBNull.Value);
                                updateMaterial.Parameters.AddWithValue("@MaterialID", materialId);
                                updateMaterial.ExecuteNonQuery();
                            }

                            using (SqlCommand deleteLinks = new SqlCommand(
                                "DELETE FROM LessonMaterials WHERE MaterialID = @MaterialID", con, tran))
                            {
                                deleteLinks.Parameters.AddWithValue("@MaterialID", materialId);
                                deleteLinks.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (SqlCommand insertMaterial = new SqlCommand(
                                @"INSERT INTO LearningMaterials (FileName, FilePath, UploadDate, UploadedBy, LevelID)
                                  OUTPUT INSERTED.MaterialID
                                  VALUES (@FileName, @FilePath, GETDATE(), @UploadedBy, @LevelID)", con, tran))
                            {
                                insertMaterial.Parameters.AddWithValue("@FileName", materialName);
                                insertMaterial.Parameters.AddWithValue("@FilePath", relativePath);
                                insertMaterial.Parameters.AddWithValue("@UploadedBy", Session["UserID"] ?? (object)DBNull.Value);
                                insertMaterial.Parameters.AddWithValue("@LevelID", (object)levelId ?? DBNull.Value);
                                materialId = Convert.ToInt32(insertMaterial.ExecuteScalar());
                            }
                        }

                        if (!isGeneralResource && lessonId > 0)
                        {
                            using (SqlCommand insertLink = new SqlCommand(
                                @"INSERT INTO LessonMaterials (LessonID, MaterialID)
                                  VALUES (@LessonID, @MaterialID)", con, tran))
                            {
                                insertLink.Parameters.AddWithValue("@LessonID", lessonId);
                                insertLink.Parameters.AddWithValue("@MaterialID", materialId);
                                insertLink.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();

                        if (!string.IsNullOrWhiteSpace(newFullPath) && File.Exists(newFullPath))
                        {
                            File.Delete(newFullPath);
                        }

                        throw;
                    }
                }

                if (isEdit && fileUpload.HasFile && !string.IsNullOrWhiteSpace(oldRelativePath))
                {
                    string oldFullPath = Server.MapPath("~/" + oldRelativePath.TrimStart('/'));
                    if (File.Exists(oldFullPath))
                    {
                        File.Delete(oldFullPath);
                    }
                }

                ShowMessage(isEdit ? "Material updated successfully." : "Material uploaded successfully.", true);
                ResetForm();
                LoadMaterials();
            }
            catch (Exception ex)
            {
                ShowMessage("Error saving material: " + ex.Message, false);
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        protected void gvMaterials_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int materialId;
            if (!int.TryParse(Convert.ToString(e.CommandArgument), out materialId))
            {
                return;
            }

            if (e.CommandName == "editMaterial")
            {
                LoadMaterialForEdit(materialId);
            }
            else if (e.CommandName == "deleteMaterial")
            {
                DeleteMaterial(materialId);
            }
        }

        private void LoadMaterialForEdit(int materialId)
        {
            using (SqlConnection con = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT TOP 1
                      lm.MaterialID,
                      lm.FileName,
                      lm.FilePath,
                      lm.LevelID,
                      lv.LevelName,
                      ISNULL(ls.LessonID, 0) AS LessonID,
                      ISNULL(ls.LevelID, 0) AS LessonLevelID
                  FROM LearningMaterials lm
                  LEFT JOIN Levels lv ON lm.LevelID = lv.LevelID
                  LEFT JOIN LessonMaterials lsm ON lm.MaterialID = lsm.MaterialID
                  LEFT JOIN Lessons ls ON lsm.LessonID = ls.LessonID
                  WHERE lm.MaterialID = @MaterialID", con))
            {
                cmd.Parameters.AddWithValue("@MaterialID", materialId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                    {
                        ShowMessage("Material not found.", false);
                        return;
                    }

                    txtMaterialName.Text = Convert.ToString(dr["FileName"]);
                    hfEditMaterialID.Value = Convert.ToString(dr["MaterialID"]);
                    hfOldFilePath.Value = Convert.ToString(dr["FilePath"]);

                    string currentFilePath = Convert.ToString(dr["FilePath"]);
                    pnlCurrentFile.Visible = !string.IsNullOrWhiteSpace(currentFilePath);
                    if (pnlCurrentFile.Visible)
                    {
                        lnkCurrentFile.Text = Path.GetFileName(currentFilePath);
                        lnkCurrentFile.NavigateUrl = ResolveUrl("~/" + currentFilePath.TrimStart('/'));
                    }
                    else
                    {
                        lnkCurrentFile.Text = string.Empty;
                        lnkCurrentFile.NavigateUrl = string.Empty;
                    }

                    int lessonId = Convert.ToInt32(dr["LessonID"]);
                    int storedLevelId = dr["LevelID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["LevelID"]);
                    int lessonLevelId = Convert.ToInt32(dr["LessonLevelID"]);
                    string levelName = dr["LevelName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["LevelName"]);

                    bool isGeneralResource = (dr["LevelID"] == DBNull.Value) && lessonId == 0;

                    if (isGeneralResource)
                    {
                        LoadUploadLevels(GeneralResourceValue, null, null);
                        LoadUploadLessons(GeneralResourceValue, null);
                    }
                    else
                    {
                        int actualLevelId = storedLevelId > 0 ? storedLevelId : lessonLevelId;
                        string selectedLevelValue = actualLevelId > 0 ? actualLevelId.ToString() : string.Empty;

                        LoadUploadLevels(selectedLevelValue, actualLevelId > 0 ? (int?)actualLevelId : null, levelName);
                        LoadUploadLessons(selectedLevelValue, lessonId > 0 ? lessonId.ToString() : null);
                    }

                    btnUpload.Text = "Update Material";
                    btnCancelEdit.Visible = true;

                    ShowMessage("Edit mode enabled.", true);
                }
            }
        }

        private void DeleteMaterial(int materialId)
        {
            string filePath = string.Empty;

            try
            {
                using (SqlConnection con = CreateConnection())
                {
                    con.Open();
                    SqlTransaction tran = con.BeginTransaction();

                    try
                    {
                        using (SqlCommand getFile = new SqlCommand(
                            "SELECT FilePath FROM LearningMaterials WHERE MaterialID = @MaterialID", con, tran))
                        {
                            getFile.Parameters.AddWithValue("@MaterialID", materialId);
                            object result = getFile.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                            {
                                filePath = Convert.ToString(result);
                            }
                        }

                        using (SqlCommand deleteLinks = new SqlCommand(
                            "DELETE FROM LessonMaterials WHERE MaterialID = @MaterialID", con, tran))
                        {
                            deleteLinks.Parameters.AddWithValue("@MaterialID", materialId);
                            deleteLinks.ExecuteNonQuery();
                        }

                        using (SqlCommand deleteMaterial = new SqlCommand(
                            "DELETE FROM LearningMaterials WHERE MaterialID = @MaterialID", con, tran))
                        {
                            deleteMaterial.Parameters.AddWithValue("@MaterialID", materialId);
                            deleteMaterial.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                if (!string.IsNullOrWhiteSpace(filePath))
                {
                    string fullPath = Server.MapPath("~/" + filePath.TrimStart('/'));
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }

                if (hfEditMaterialID.Value == materialId.ToString())
                {
                    ResetForm();
                }

                ShowMessage("Material deleted successfully.", true);
                LoadMaterials();
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting material: " + ex.Message, false);
            }
        }

        private void ResetForm()
        {
            ClearFormFields();
            LoadUploadLevels(null, null, null);
            LoadUploadLessons(string.Empty, null);
        }

        private void ClearFormFields()
        {
            txtMaterialName.Text = string.Empty;
            hfEditMaterialID.Value = string.Empty;
            hfOldFilePath.Value = string.Empty;
            btnUpload.Text = "Upload Material";
            btnCancelEdit.Visible = false;

            pnlCurrentFile.Visible = false;
            lnkCurrentFile.Text = string.Empty;
            lnkCurrentFile.NavigateUrl = string.Empty;
        }
    }
}