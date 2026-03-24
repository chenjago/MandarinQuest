<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TakeQuiz.aspx.cs" Inherits="MandarinQuest.TakeQuiz" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Take Quiz</title>

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
            width:900px;
            margin:auto;
            margin-top:30px;
            margin-bottom:30px;
            background:white;
            padding:25px;
            border-radius:10px;
            box-shadow:0 4px 10px rgba(0,0,0,0.15);
        }

        .quiz-title{
            font-size:22px;
            font-weight:bold;
            margin-bottom:8px;
        }

        .quiz-note{
            color:#666;
            margin-bottom:20px;
        }

        .question-block{
            margin-bottom:25px;
            padding:18px;
            border:1px solid #ddd;
            border-radius:8px;
            background:#fafafa;
        }

        .question-text{
            font-weight:bold;
            margin-bottom:12px;
            font-size:17px;
        }

        .option-item{
            margin-bottom:8px;
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
            display:block;
            margin-bottom:15px;
            color:#b30000;
            font-weight:600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="header">
            Take Quiz
        </div>

        <div class="container">

            <div class="quiz-title">
                <asp:Label ID="lblQuizTitle" runat="server"></asp:Label>
            </div>

            <div class="quiz-note">
                Passing mark: 4 out of 5
            </div>

            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="question-block">
                        <div class="question-text">
                            <%# Container.ItemIndex + 1 %>. <%# Eval("QuestionText") %>
                        </div>

                        <asp:HiddenField ID="hfQuestionID" runat="server" Value='<%# Eval("QuestionID") %>' />

                        <asp:RadioButtonList ID="rblOptions" runat="server" RepeatDirection="Vertical"></asp:RadioButtonList>
                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button
                ID="btnSubmit"
                runat="server"
                Text="Submit Quiz"
                CssClass="btn"
                OnClick="btnSubmit_Click" />

            <asp:Button
                ID="btnCancel"
                runat="server"
                Text="Back"
                CssClass="btn"
                OnClick="btnCancel_Click"
                CausesValidation="false" />

        </div>

    </form>
</body>
</html>