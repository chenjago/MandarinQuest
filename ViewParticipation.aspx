<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewParticipation.aspx.cs" Inherits="MandarinQuest.ViewParticipation" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Participation</title>
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

        .header{
            font-size:34px;
            font-weight:700;
            color:#b30000;
            text-align:center;
            margin-top:30px;
        }

        .subheader{
            text-align:center;
            color:#666;
            margin-top:8px;
            margin-bottom:20px;
        }

        .container{
            width:1120px;
            margin:auto;
            padding:30px;
        }

        .section{
            background:white;
            padding:22px;
            border-radius:12px;
            box-shadow:0 4px 12px rgba(0,0,0,0.10);
            margin-bottom:28px;
        }

        .section h3{
            margin-top:0;
            margin-bottom:18px;
            color:#111;
            font-size:18px;
        }

        .summary-grid{
            display:grid;
            grid-template-columns:repeat(4, 1fr);
            gap:18px;
        }

        .summary-card{
            background:linear-gradient(180deg, #fff, #fff8f8);
            border:1px solid #f0d1d1;
            border-radius:12px;
            padding:18px;
        }

        .summary-label{
            color:#777;
            font-size:14px;
            margin-bottom:8px;
        }

        .summary-value{
            color:#b30000;
            font-size:28px;
            font-weight:700;
        }

        .filter-grid{
            display:grid;
            grid-template-columns:1fr 1fr 1fr 1.2fr auto auto;
            gap:14px;
            align-items:end;
        }

        .field-label{
            font-weight:600;
            display:block;
            margin-bottom:6px;
        }

        select, input[type=text]{
            width:100%;
            padding:10px;
            border:1px solid #ccc;
            border-radius:6px;
            box-sizing:border-box;
            font-family:'Segoe UI';
        }

        .btn{
            background:#b30000;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:6px;
            cursor:pointer;
            font-weight:600;
            min-width:100px;
        }

        .btn:hover{
            background:#d40000;
        }

        .btn-secondary{
            background:#666;
        }

        .btn-secondary:hover{
            background:#444;
        }

        .grid{
            width:100%;
            border-collapse:collapse;
        }

        .grid th{
            background:#b30000;
            color:white;
            padding:12px 10px;
            text-align:left;
        }

        .grid td{
            padding:12px 10px;
            border-bottom:1px solid #ddd;
            vertical-align:middle;
        }

        .grid tr:nth-child(even) td{
            background:#fcfcfc;
        }

        .status-badge{
            display:inline-block;
            padding:6px 12px;
            border-radius:16px;
            font-size:12px;
            font-weight:700;
            color:white;
        }

        .status-completed{
            background:#2e7d32;
        }

        .status-inprogress{
            background:#ef6c00;
        }

        .status-notstarted{
            background:#757575;
        }

        .status-other{
            background:#1565c0;
        }

        .empty-box{
            margin-top:14px;
            color:#888;
            font-style:italic;
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

            <div class="header">Student Participation</div>
            <div class="subheader">Track participation by level, lesson, student, and completion status</div>

            <div class="section">
                <h3>Overview</h3>

                <div class="summary-grid">
                    <div class="summary-card">
                        <div class="summary-label">Total Records</div>
                        <asp:Label ID="lblTotalRecords" runat="server" CssClass="summary-value" Text="0"></asp:Label>
                    </div>

                    <div class="summary-card">
                        <div class="summary-label">Unique Students</div>
                        <asp:Label ID="lblTotalStudents" runat="server" CssClass="summary-value" Text="0"></asp:Label>
                    </div>

                    <div class="summary-card">
                        <div class="summary-label">Completed Records</div>
                        <asp:Label ID="lblCompletedRecords" runat="server" CssClass="summary-value" Text="0"></asp:Label>
                    </div>

                    <div class="summary-card">
                        <div class="summary-label">Completion Rate</div>
                        <asp:Label ID="lblCompletionRate" runat="server" CssClass="summary-value" Text="0%"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="section">
                <h3>Filter</h3>

                <div class="filter-grid">
                    <div>
                        <label class="field-label">Level</label>
                        <asp:DropDownList
                            ID="ddlLevels"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLevels_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div>
                        <label class="field-label">Lesson</label>
                        <asp:DropDownList
                            ID="ddlLessons"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlLessons_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>

                    <div>
                        <label class="field-label">Status</label>
                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                            <asp:ListItem Text="All Status" Value=""></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                            <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                            <asp:ListItem Text="Not Started" Value="Not Started"></asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div>
                        <label class="field-label">Search Student</label>
                        <asp:TextBox ID="txtSearchStudent" runat="server" placeholder="Enter student name"></asp:TextBox>
                    </div>

                    <div>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn" OnClick="btnSearch_Click" />
                    </div>

                    <div>
                        <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
                    </div>
                </div>
            </div>

            <div class="section">
                <h3>Participation Records</h3>

                <asp:GridView
                    ID="dgvParticipation"
                    runat="server"
                    CssClass="grid"
                    AutoGenerateColumns="false"
                    OnRowDataBound="dgvParticipation_RowDataBound"
                    EmptyDataText="No participation records found.">

                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Student" />
                        <asp:BoundField DataField="LevelName" HeaderText="Level" />
                        <asp:BoundField DataField="LessonTitle" HeaderText="Lesson" />

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CompletionStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Completed On">
                            <ItemTemplate>
                                <asp:Label ID="lblCompletedOn" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>

        </div>

    </form>
</body>
</html>