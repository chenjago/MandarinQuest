<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuizReview.aspx.cs" Inherits="MandarinQuest.QuizReview" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Quiz Review</title>

    <style>
        body{
            font-family:'Segoe UI';
            background:#f5f5f5;
            margin:0;
        }

        .header{
            background:#b30000;
            color:white;
            padding:18px;
            text-align:center;
            font-size:22px;
        }

        .container{
            width:950px;
            margin:auto;
            margin-top:30px;
            margin-bottom:30px;
            background:white;
            padding:25px;
            border-radius:10px;
            box-shadow:0 4px 10px rgba(0,0,0,0.15);
        }

        .result-box{
            background:#fafafa;
            border:1px solid #ddd;
            border-radius:8px;
            padding:15px;
            margin-bottom:20px;
        }

        .question-card{
            border:1px solid #ddd;
            border-radius:8px;
            padding:18px;
            margin-bottom:15px;
            background:#fcfcfc;
        }

        .question-title{
            font-weight:bold;
            font-size:17px;
            margin-bottom:10px;
        }

        .line{
            margin-bottom:6px;
        }

        .correct{
            color:green;
            font-weight:600;
        }

        .wrong{
            color:#b30000;
            font-weight:600;
        }

        .btn{
            background:#b30000;
            color:white;
            border:none;
            padding:10px 16px;
            border-radius:6px;
            cursor:pointer;
            margin-right:10px;
        }

        .btn:hover{
            background:#ff3333;
        }

        .message{
            color:#b30000;
            font-weight:600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="header">
            Quiz Review
        </div>

        <div class="container">

            <div class="result-box">
                <asp:Label ID="lblQuizTitle" runat="server" Font-Size="20px" Font-Bold="true"></asp:Label>
                <br /><br />
                <asp:Label ID="lblResult" runat="server" Font-Size="16px" Font-Bold="true"></asp:Label>
                <br /><br />
                <asp:Label ID="lblAttemptDate" runat="server"></asp:Label>
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

            <asp:Repeater ID="rptReview" runat="server">
                <ItemTemplate>
                    <div class="question-card">
                        <div class="question-title">
                            <%# Container.ItemIndex + 1 %>. <%# Eval("QuestionText") %>
                        </div>

                        <div class="line">
                            Your Answer:
                            <span class='<%# Convert.ToBoolean(Eval("IsCorrect")) ? "correct" : "wrong" %>'>
                                <%# Eval("SelectedOption") %>
                            </span>
                        </div>

                        <div class="line">
                            Correct Answer:
                            <span class="correct">
                                <%# Eval("CorrectOption") %>
                            </span>
                        </div>

                        <div class="line">
                            Explanation:
                            <%# Eval("Explanation") %>
                        </div>

                        <div class="line">
                            Result:
                            <span class='<%# Convert.ToBoolean(Eval("IsCorrect")) ? "correct" : "wrong" %>'>
                                <%# Convert.ToBoolean(Eval("IsCorrect")) ? "Correct" : "Wrong" %>
                            </span>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button
                ID="btnBack"
                runat="server"
                Text="Back to Materials"
                CssClass="btn"
                OnClick="btnBack_Click" />

            <asp:Button
                ID="btnRetake"
                runat="server"
                Text="Retake Quiz"
                CssClass="btn"
                Visible="false"
                OnClick="btnRetake_Click" />

        </div>

    </form>
</body>
</html>