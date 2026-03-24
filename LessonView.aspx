<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LessonView.aspx.cs" Inherits="MandarinQuest.LessonView" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Lessons</title>
    <style>
        body{
            font-family:'Segoe UI';
            background:#f5f5f5;
            margin:0;
        }

        .topbar{
            background:#b30000;
            color:white;
            padding:18px 30px;
            font-size:24px;
            font-weight:700;
        }

        .container{
            max-width:1100px;
            margin:30px auto;
            padding:0 20px 30px 20px;
        }

        .page-title{
            font-size:30px;
            font-weight:700;
            margin-bottom:8px;
            color:#222;
        }

        .page-subtitle{
            color:#666;
            margin-bottom:24px;
        }

        .message{
            display:block;
            padding:12px 16px;
            border-radius:8px;
            margin-bottom:20px;
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

        .lesson-card{
            background:white;
            padding:22px;
            border-radius:14px;
            margin-bottom:18px;
            box-shadow:0 4px 10px rgba(0,0,0,0.08);
            display:flex;
            justify-content:space-between;
            align-items:flex-start;
            gap:20px;
        }

        .lesson-left{
            flex:1;
        }

        .lesson-title{
            font-size:22px;
            font-weight:700;
            color:#1f2937;
            margin-bottom:8px;
        }

        .lesson-desc{
            color:#555;
            margin-bottom:14px;
            line-height:1.5;
        }

        .meta-row{
            display:flex;
            flex-wrap:wrap;
            gap:10px;
            margin-bottom:10px;
        }

        .badge{
            display:inline-block;
            padding:7px 12px;
            border-radius:999px;
            font-size:12px;
            font-weight:700;
        }

        .level-badge{
            background:#eef2ff;
            color:#4338ca;
        }

        .status-completed{
            background:#e8f5e9;
            color:#2e7d32;
        }

        .status-takequiz{
            background:#fff3e0;
            color:#ef6c00;
        }

        .status-retake{
            background:#ffebee;
            color:#c62828;
        }

        .status-passed{
            background:#e8f5e9;
            color:#2e7d32;
        }

        .status-locked{
            background:#f3f4f6;
            color:#6b7280;
        }

        .quiz-info{
            font-size:14px;
            color:#444;
            margin-top:4px;
        }

        .lesson-right{
            display:flex;
            flex-direction:column;
            gap:10px;
            min-width:200px;
        }

        .btn{
            padding:10px 16px;
            border:none;
            border-radius:8px;
            cursor:pointer;
            font-weight:600;
            font-size:14px;
            width:100%;
        }

        .open{
            background:#2563eb;
            color:white;
        }

        .open:hover{
            background:#1d4ed8;
        }

        .btn-orange{
            background:#ef6c00;
            color:white;
        }

        .btn-orange:hover{
            background:#d65b00;
        }

        .btn-red{
            background:#dc2626;
            color:white;
        }

        .btn-red:hover{
            background:#b91c1c;
        }

        .btn-green{
            background:#16a34a;
            color:white;
        }

        .btn-green:hover{
            background:#15803d;
        }

        .btn-completed{
            background:#c8e6c9;
            color:#1b5e20;
            cursor:not-allowed;
        }

        .btn-disabled{
            background:#e5e7eb;
            color:#6b7280;
            cursor:not-allowed;
        }

        .empty-box{
            background:white;
            padding:24px;
            border-radius:12px;
            color:#666;
            box-shadow:0 4px 10px rgba(0,0,0,0.08);
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <div class="topbar">MandarinQuest - Lessons</div>

        <div class="container">
            <div class="page-title">Lesson View</div>
            <div class="page-subtitle">Only lessons from the selected level are shown here. Pass the quiz with at least 4/5 before completing the lesson.</div>

            <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

            <asp:Repeater ID="rptLessons" runat="server" OnItemDataBound="rptLessons_ItemDataBound">
                <ItemTemplate>
                    <div class="lesson-card">
                        <div class="lesson-left">
                            <div class="lesson-title"><%# Eval("LessonTitle") %></div>
                            <div class="lesson-desc">
                                <%# string.IsNullOrWhiteSpace(Convert.ToString(Eval("Description"))) ? "No description available." : Eval("Description") %>
                            </div>

                            <div class="meta-row">
                                <span class="badge level-badge"><%# Eval("LevelName") %></span>
                                <asp:Label ID="lblLessonStatus" runat="server" CssClass="badge"></asp:Label>
                            </div>

                            <div class="quiz-info">
                                <asp:Label ID="lblQuizInfo" runat="server"></asp:Label>
                            </div>

                            <asp:HiddenField ID="hfLessonId" runat="server" Value='<%# Eval("LessonID") %>' />
                            <asp:HiddenField ID="hfHasQuiz" runat="server" Value='<%# Eval("HasQuiz") %>' />
                            <asp:HiddenField ID="hfQuizPassed" runat="server" Value='<%# Eval("QuizPassed") %>' />
                            <asp:HiddenField ID="hfQuizScore" runat="server" Value='<%# Eval("QuizScore") %>' />
                            <asp:HiddenField ID="hfStudentStatus" runat="server" Value='<%# Eval("StudentStatus") %>' />
                            <asp:HiddenField ID="hfLastQuizAttemptDate" runat="server" Value='<%# Eval("LastQuizAttemptDate") %>' />
                        </div>

                        <div class="lesson-right">
                            <asp:Button
                                ID="btnOpen"
                                runat="server"
                                Text="Open Lesson"
                                CssClass="btn open"
                                CommandName="open"
                                CommandArgument='<%# Eval("LessonID") %>'
                                OnCommand="OpenLesson" />

                            <asp:Button
                                ID="btnAction"
                                runat="server"
                                Text="Take Quiz"
                                CssClass="btn btn-orange"
                                CommandName="quiz"
                                CommandArgument='<%# Eval("LessonID") %>'
                                OnCommand="ActionLesson" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-box">
                No lessons found for this level.
            </asp:Panel>
        </div>

    </form>

</body>
</html>