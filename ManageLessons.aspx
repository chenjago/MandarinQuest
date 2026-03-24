<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLessons.aspx.cs" Inherits="MandarinQuest.ManageLessons" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Lessons</title>

    <style>
        body{
            margin:0;
            font-family:'Segoe UI';
            background:#f6f6f6;
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

        .topbar{
            padding:20px 0 0 0;
        }

        .backbtn{
            background:#444;
            color:white;
            border:none;
            width:80px;
            height:40px;
            font-size:12px;
            cursor:pointer;
            border-radius:6px;
            display:inline-flex;
            align-items:center;
            justify-content:center;
        }

        .container{
            width:1120px;
            margin:auto;
            padding-bottom:40px;
        }

        .section{
            background:white;
            padding:20px;
            margin-bottom:30px;
            border-radius:10px;
            box-shadow:0 4px 10px rgba(0,0,0,0.1);
        }

        .section h3{
            color:#b30000;
            margin-top:0;
        }

        .form-label{
            font-weight:600;
            display:block;
            margin-bottom:6px;
        }

        input, textarea, select{
            width:100%;
            padding:10px;
            margin-top:5px;
            margin-bottom:14px;
            border:1px solid #ccc;
            border-radius:6px;
            box-sizing:border-box;
            font-family:'Segoe UI';
        }

        textarea{
            resize:vertical;
            min-height:100px;
        }

        .btn{
            background:#b30000;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:5px;
            cursor:pointer;
            margin-right:8px;
        }

        .btn:hover{
            background:#ff3333;
        }

        .btnSecondary{
            background:#666;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:5px;
            cursor:pointer;
            margin-right:8px;
        }

        .btnSecondary:hover{
            background:#444;
        }

        .smallBtn{
            color:white;
            border:none;
            padding:7px 12px;
            border-radius:4px;
            cursor:pointer;
            margin-right:5px;
            margin-bottom:4px;
            font-size:12px;
        }

        .editBtn{
            background:#1976d2;
        }

        .editBtn:hover{
            background:#0d47a1;
        }

        .materialBtn{
            background:#2e7d32;
        }

        .materialBtn:hover{
            background:#1b5e20;
        }

        .quizBtn{
            background:#6a1b9a;
        }

        .quizBtn:hover{
            background:#4a148c;
        }

        .deleteBtn{
            background:#e53935;
        }

        .deleteBtn:hover{
            background:#b71c1c;
        }

        .grid{
            width:100%;
            border-collapse:collapse;
        }

        .grid th{
            background:#b30000;
            color:white;
            padding:10px;
            text-align:left;
            font-size:14px;
        }

        .grid td{
            padding:10px;
            border-bottom:1px solid #ddd;
            vertical-align:top;
            font-size:14px;
        }

        .statusBadge{
            display:inline-block;
            padding:6px 10px;
            border-radius:14px;
            font-size:12px;
            font-weight:600;
            color:white;
        }

        .statusNoMaterials{ background:#757575; }
        .statusNoQuiz{ background:#ef6c00; }
        .statusUpdated{ background:#2e7d32; }
        .statusOutdated{ background:#c62828; }

        .msg{
            display:block;
            padding:12px;
            border-radius:6px;
            margin-bottom:15px;
            font-weight:600;
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

        <div class="container">

            <div class="topbar">
                <asp:Button
                    ID="btnBack"
                    runat="server"
                    Text="⬅ Back"
                    CssClass="backbtn"
                    OnClick="btnBack_Click" />
            </div>

            <div class="section">
                <h3>Filter by Level</h3>

                <asp:DropDownList
                    ID="ddlClasses"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlClasses_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <div class="section">
                <h3>Lesson Form</h3>

                <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                <asp:HiddenField ID="hfLessonID" runat="server" />

                <label class="form-label">Lesson Title</label>
                <asp:TextBox ID="txtLessonTitle" runat="server"></asp:TextBox>

                <label class="form-label">Description</label>
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>

                <asp:Button
                    ID="btnAddLesson"
                    runat="server"
                    Text="Add Lesson"
                    CssClass="btn"
                    OnClick="btnAddLesson_Click" />

                <asp:Button
                    ID="btnUpdateLesson"
                    runat="server"
                    Text="Update Lesson"
                    CssClass="btn"
                    Visible="false"
                    OnClick="btnUpdateLesson_Click" />

                <asp:Button
                    ID="btnCancelEdit"
                    runat="server"
                    Text="Cancel"
                    CssClass="btnSecondary"
                    Visible="false"
                    OnClick="btnCancelEdit_Click" />
            </div>

            <div class="section">
                <h3>Lessons</h3>

                <asp:GridView
                    ID="gvLessons"
                    runat="server"
                    CssClass="grid"
                    AutoGenerateColumns="false"
                    OnRowCommand="gvLessons_RowCommand">

                    <Columns>
                        <asp:BoundField DataField="LessonID" HeaderText="ID" />
                        <asp:BoundField DataField="LevelName" HeaderText="Level" />
                        <asp:BoundField DataField="LessonTitle" HeaderText="Lesson" />
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="MaterialCount" HeaderText="Materials" />
                        <asp:BoundField DataField="CreatedDateDisplay" HeaderText="Created" />

                        <asp:TemplateField HeaderText="Quiz Status">
                            <ItemTemplate>
                                <span class='statusBadge <%# GetQuizStatusCss(Eval("QuizStatus").ToString()) %>'>
                                    <%# Eval("QuizStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button
                                    runat="server"
                                    Text="Edit"
                                    CssClass="smallBtn editBtn"
                                    CommandName="EditLesson"
                                    CommandArgument='<%# Eval("LessonID") %>' />

                                <asp:Button
                                    runat="server"
                                    Text="Materials"
                                    CssClass="smallBtn materialBtn"
                                    CommandName="ManageMaterials"
                                    CommandArgument='<%# Eval("LessonID") %>' />

                                <asp:Button
                                    runat="server"
                                    Text="Quiz"
                                    CssClass="smallBtn quizBtn"
                                    CommandName="ManageQuiz"
                                    CommandArgument='<%# Eval("LessonID") %>' />

                                <asp:Button
                                    runat="server"
                                    Text="Delete"
                                    CssClass="smallBtn deleteBtn"
                                    CommandName="DeleteLesson"
                                    CommandArgument='<%# Eval("LessonID") %>'
                                    OnClientClick="return confirm('Are you sure you want to delete this lesson?');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>

        </div>

    </form>
</body>
</html> 