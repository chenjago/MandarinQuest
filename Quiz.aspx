<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Quiz.aspx.cs" Inherits="MandarinQuest.Quiz" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lesson Quiz</title>
    <style>
        body{
            margin:0;
            font-family:'Segoe UI';
            background:#f6f6f6;
            color:#222;
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

        .container{
            width:1000px;
            margin:30px auto;
        }

        .topbar{
            margin-bottom:20px;
        }

        .backbtn{
            background:#444;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:6px;
            cursor:pointer;
            font-size:13px;
        }

        .backbtn:hover{
            background:#222;
        }

        .card{
            background:white;
            border-radius:12px;
            padding:24px;
            margin-bottom:24px;
            box-shadow:0 4px 12px rgba(0,0,0,0.08);
        }

        .title{
            font-size:30px;
            font-weight:700;
            color:#b30000;
            margin-bottom:8px;
        }

        .subtitle{
            color:#666;
            font-size:15px;
            margin-bottom:10px;
        }

        .lessonInfo{
            display:grid;
            grid-template-columns:1fr 1fr;
            gap:16px;
            margin-top:18px;
        }

        .infoBox{
            background:#fafafa;
            border:1px solid #eee;
            border-radius:10px;
            padding:16px;
        }

        .infoLabel{
            font-size:13px;
            color:#777;
            margin-bottom:5px;
        }

        .infoValue{
            font-size:18px;
            font-weight:600;
        }

        .statusBadge{
            display:inline-block;
            padding:8px 14px;
            border-radius:20px;
            color:white;
            font-size:12px;
            font-weight:700;
        }

        .statusNoQuiz{
            background:#ef6c00;
        }

        .statusReady{
            background:#2e7d32;
        }

        .statusDraft{
            background:#1565c0;
        }

        .btn{
            background:#b30000;
            color:white;
            border:none;
            padding:11px 18px;
            border-radius:6px;
            cursor:pointer;
            font-size:14px;
            margin-right:10px;
            margin-top:10px;
        }

        .btn:hover{
            background:#8f0000;
        }

        .btnSecondary{
            background:#6a1b9a;
        }

        .btnSecondary:hover{
            background:#4a148c;
        }

        .btnGray{
            background:#555;
        }

        .btnGray:hover{
            background:#333;
        }

        .message{
            padding:14px 16px;
            border-radius:8px;
            margin-bottom:18px;
            font-weight:600;
            display:block;
        }

        .success{
            background:#e8f5e9;
            border:1px solid #a5d6a7;
            color:#2e7d32;
        }

        .error{
            background:#ffebee;
            border:1px solid #ef9a9a;
            color:#c62828;
        }

        .questionCard{
            background:#fff;
            border:1px solid #ececec;
            border-radius:12px;
            padding:20px;
            margin-bottom:18px;
        }

        .questionHeader{
            display:flex;
            justify-content:space-between;
            align-items:center;
            margin-bottom:12px;
        }

        .questionNo{
            font-size:18px;
            font-weight:700;
            color:#b30000;
        }

        .correctBadge{
            background:#e8f5e9;
            color:#2e7d32;
            padding:6px 10px;
            border-radius:14px;
            font-size:12px;
            font-weight:700;
        }

        .questionText{
            font-size:18px;
            font-weight:600;
            margin-bottom:16px;
            line-height:1.5;
        }

        .options{
            display:grid;
            grid-template-columns:1fr 1fr;
            gap:12px;
            margin-bottom:14px;
        }

        .optionBox{
            border:1px solid #ddd;
            border-radius:10px;
            padding:12px;
            background:#fafafa;
        }

        .optionLabel{
            font-weight:700;
            color:#b30000;
            margin-bottom:6px;
        }

        .explanation{
            background:#f9f9f9;
            border-left:4px solid #b30000;
            padding:12px;
            border-radius:6px;
            color:#444;
        }

        .emptyState{
            text-align:center;
            color:#666;
            padding:30px 10px;
            font-size:16px;
        }

        .sectionTitle{
            font-size:24px;
            font-weight:700;
            color:#b30000;
            margin-bottom:18px;
        }

        .actionRow{
            margin-top:10px;
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
                <asp:Button ID="btnBack" runat="server" Text="⬅ Back to Lessons" CssClass="backbtn" OnClick="btnBack_Click" />
            </div>

            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

            <div class="card">
                <div class="title">Lesson Quiz</div>
                <div class="subtitle">Quiz management for the selected lesson</div>

                <div class="lessonInfo">
                    <div class="infoBox">
                        <div class="infoLabel">Lesson ID</div>
                        <asp:Label ID="lblLessonID" runat="server" CssClass="infoValue"></asp:Label>
                    </div>

                    <div class="infoBox">
                        <div class="infoLabel">Lesson Title</div>
                        <asp:Label ID="lblLessonTitle" runat="server" CssClass="infoValue"></asp:Label>
                    </div>

                    <div class="infoBox">
                        <div class="infoLabel">Quiz Title</div>
                        <asp:Label ID="lblQuizTitle" runat="server" CssClass="infoValue"></asp:Label>
                    </div>

                    <div class="infoBox">
                        <div class="infoLabel">Quiz Status</div>
                        <asp:Label ID="lblQuizStatus" runat="server"></asp:Label>
                    </div>
                </div>

                <div class="actionRow">
                    <asp:Button ID="btnGenerateDummyQuiz" runat="server" Text="Generate AI Quiz" CssClass="btn" OnClick="btnGenerateDummyQuiz_Click" />
                    <asp:Button ID="btnRegenerateQuiz" runat="server" Text="Regenerate Quiz" CssClass="btn btnSecondary" OnClick="btnRegenerateQuiz_Click" />
                    <asp:Button ID="btnApproveQuiz" runat="server" Text="Approve Quiz" CssClass="btn" OnClick="btnApproveQuiz_Click" />
                    <asp:Button ID="btnDeleteQuiz" runat="server" Text="Delete Quiz" CssClass="btn btnGray" OnClick="btnDeleteQuiz_Click" OnClientClick="return confirm('Are you sure you want to delete this quiz?');" />
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="btn btnGray" OnClick="btnRefresh_Click" />
                </div>
            </div>

            <div class="card">
                <div class="sectionTitle">Quiz Questions</div>

                <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                    <ItemTemplate>
                        <div class="questionCard">
                            <div class="questionHeader">
                                <div class="questionNo">Question <%# Eval("QuestionOrder") %></div>
                                <div class="correctBadge">Correct Answer: <%# Eval("CorrectOption") %></div>
                            </div>

                            <div class="questionText"><%# Eval("QuestionText") %></div>

                            <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />

                            <asp:Repeater ID="rptOptions" runat="server">
                                <HeaderTemplate>
                                    <div class="options">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="optionBox">
                                        <div class="optionLabel"><%# Eval("OptionLabel") %></div>
                                        <div><%# Eval("OptionText") %></div>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>

                            <div class="explanation">
                                <strong>Explanation:</strong>
                                <%# string.IsNullOrWhiteSpace(Convert.ToString(Eval("Explanation"))) ? "No explanation provided." : Convert.ToString(Eval("Explanation")) %>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Panel ID="pnlNoQuiz" runat="server" Visible="false">
                    <div class="emptyState">
                        No quiz found for this lesson yet. Click <strong>Generate AI Quiz</strong> to create one from the lesson materials.
                    </div>
                </asp:Panel>
            </div>

        </div>
    </form>
</body>
</html>