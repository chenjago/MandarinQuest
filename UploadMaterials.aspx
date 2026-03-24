<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadMaterials.aspx.cs" Inherits="MandarinQuest.UploadMaterials" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Materials</title>
    <style>
        body{
            font-family:'Segoe UI';
            background:#fff5f5;
            margin:0;
        }

        .navbar{
            background:#b30000;
            padding:15px;
            text-align:center;
        }

        .navbar a{
            color:white;
            margin:0 15px;
            text-decoration:none;
            font-weight:bold;
        }

        .navbar a:hover{
            text-decoration:underline;
        }

        .header{
            background:#b30000;
            color:white;
            padding:20px;
            text-align:center;
            font-size:24px;
            font-weight:700;
        }

        .container{
            width:1120px;
            margin:30px auto;
        }

        .section{
            background:white;
            padding:20px;
            border-radius:12px;
            box-shadow:0 4px 12px rgba(0,0,0,0.12);
            margin-bottom:30px;
        }

        .section h3{
            color:#b30000;
            margin-top:0;
            margin-bottom:18px;
        }

        .message{
            padding:12px 14px;
            border-radius:8px;
            margin-bottom:20px;
            font-weight:600;
            display:block;
        }

        .success{
            background:#e8f5e9;
            color:#2e7d32;
            border:1px solid #a5d6a7;
        }

        .error{
            background:#ffebee;
            color:#c62828;
            border:1px solid #ef9a9a;
        }

        .form-grid{
            display:grid;
            grid-template-columns:1fr 1fr;
            gap:18px;
        }

        .field-full{
            grid-column:1 / -1;
        }

        .field-label{
            font-weight:600;
            margin-bottom:6px;
            display:block;
        }

        input[type=text], select{
            width:100%;
            padding:10px;
            margin-top:5px;
            border:1px solid #d0d0d0;
            border-radius:6px;
            box-sizing:border-box;
            font-family:'Segoe UI';
        }

        input[type=file]{
            margin-top:8px;
        }

        .btn{
            background:#b30000;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:6px;
            cursor:pointer;
            font-weight:600;
            min-width:120px;
        }

        .btn:hover{
            background:#ff3333;
        }

        .btn-secondary{
            background:#666;
        }

        .btn-secondary:hover{
            background:#444;
        }

        .btn-edit{
            background:#1976d2;
        }

        .btn-edit:hover{
            background:#125ca1;
        }

        .btn-delete{
            background:#d32f2f;
        }

        .btn-delete:hover{
            background:#a61f1f;
        }

        .btn-search{
            background:#2e7d32;
        }

        .btn-search:hover{
            background:#1f5f24;
        }

        .action-row{
            margin-top:16px;
            display:flex;
            gap:10px;
            flex-wrap:wrap;
        }

        .filter-grid{
            display:grid;
            grid-template-columns:220px 240px 1fr auto auto;
            gap:12px;
            align-items:end;
            margin-bottom:18px;
        }

        .grid{
            width:100%;
            border-collapse:collapse;
        }

        .grid th{
            background:#b30000;
            color:white;
            padding:10px 8px;
            text-align:left;
        }

        .grid td{
            padding:10px 8px;
            border:1px solid #d0d0d0;
            vertical-align:middle;
        }

        .grid tr:nth-child(even) td{
            background:#fff9f9;
        }

        .actions-cell{
            white-space:nowrap;
        }

        .small-btn{
            padding:8px 12px;
            font-size:13px;
            min-width:80px;
            margin-right:6px;
        }

        .empty-note{
            color:#666;
            font-style:italic;
            margin-top:8px;
        }

        .current-file-box{
            margin-top:10px;
            padding:10px 12px;
            background:#fff9f9;
            border:1px solid #f0caca;
            border-radius:8px;
        }

        .current-file-box a{
            color:#1976d2;
            text-decoration:none;
            font-weight:600;
        }

        .current-file-box a:hover{
            text-decoration:underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="navbar">
            <a href="TeacherPage.aspx">Dashboard</a>
            <a href="ManageLevels.aspx">Manage Levels</a>
            <a href="ManageLessons.aspx">Manage Lessons</a>
            <a href="UploadMaterials.aspx">Upload Materials</a>
            <a href="ScheduleSessions.aspx">Schedule Session</a>
            <a href="ViewParticipation.aspx">Participation</a>
        </div>

        <div class="header">Teacher – Upload Materials</div>

        <div class="container">

            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

            <div class="section">
                <h3>Upload Material</h3>

                <asp:HiddenField ID="hfEditMaterialID" runat="server" />
                <asp:HiddenField ID="hfOldFilePath" runat="server" />

                <div class="form-grid">
                    <div>
                        <label class="field-label">Material Name</label>
                        <asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>
                    </div>

                    <div>
                        <label class="field-label">Select Level / GR</label>
                        <asp:DropDownList ID="ddlLevelsUpload" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlLevelsUpload_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="field-full">
                        <label class="field-label">Select Lesson</label>
                        <asp:DropDownList ID="ddlLessonsUpload" runat="server"></asp:DropDownList>
                        <div class="empty-note">For GR, lesson is not required.</div>
                    </div>

                    <div class="field-full">
                        <label class="field-label">File</label>
                        <asp:FileUpload ID="fileUpload" runat="server" />
                        <div class="empty-note">When editing, leave empty to keep the current file. Upload a new file to replace it automatically.</div>

                        <asp:Panel ID="pnlCurrentFile" runat="server" Visible="false" CssClass="current-file-box">
                            Current file:
                            <asp:HyperLink ID="lnkCurrentFile" runat="server" Target="_blank"></asp:HyperLink>
                        </asp:Panel>
                    </div>
                </div>

                <div class="action-row">
                    <asp:Button ID="btnUpload" runat="server" Text="Upload Material" CssClass="btn" OnClick="btnUpload_Click" />
                    <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" CssClass="btn btn-secondary" Visible="false" OnClick="btnCancelEdit_Click" />
                </div>
            </div>

            <div class="section">
                <h3>Uploaded Materials</h3>

                <div class="filter-grid">
                    <div>
                        <label class="field-label">Filter Level / GR</label>
                        <asp:DropDownList ID="ddlFilterLevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterLevel_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div>
                        <label class="field-label">Filter Lesson</label>
                        <asp:DropDownList ID="ddlFilterLesson" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterLesson_SelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div>
                        <label class="field-label">Search File Name</label>
                        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                    </div>

                    <div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-search" OnClick="btnSearch_Click" />
                    </div>

                    <div>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                    </div>
                </div>

                <asp:GridView
                    ID="gvMaterials"
                    runat="server"
                    CssClass="grid"
                    AutoGenerateColumns="False"
                    OnRowCommand="gvMaterials_RowCommand"
                    EmptyDataText="No materials found.">

                    <Columns>
                        <asp:BoundField DataField="MaterialID" HeaderText="ID" />
                        <asp:BoundField DataField="LevelName" HeaderText="Level" />
                        <asp:BoundField DataField="LessonTitle" HeaderText="Lesson" />
                        <asp:BoundField DataField="FileName" HeaderText="File Name" />
                        <asp:BoundField DataField="UploadDate" HeaderText="Upload Date" DataFormatString="{0:dd/MM/yyyy hh:mm tt}" />

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button
                                    ID="btnEdit"
                                    runat="server"
                                    Text="Edit"
                                    CssClass="btn btn-edit small-btn"
                                    CommandName="editMaterial"
                                    CommandArgument='<%# Eval("MaterialID") %>' />

                                <asp:Button
                                    ID="btnDelete"
                                    runat="server"
                                    Text="Delete"
                                    CssClass="btn btn-delete small-btn"
                                    CommandName="deleteMaterial"
                                    CommandArgument='<%# Eval("MaterialID") %>'
                                    OnClientClick="return confirm('Delete this material?');" />
                            </ItemTemplate>
                            <ItemStyle CssClass="actions-cell" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>

    </form>
</body>
</html>