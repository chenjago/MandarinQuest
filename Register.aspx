<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="MandarinQuest.Register" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register - MandarinQuest</title>

    <style>
        body{
            margin:0;
            font-family:'Segoe UI';
            background:#fff5f5;
            display:flex;
            justify-content:center;
            align-items:center;
            height:100vh;
        }

        .card{
            width:380px;
            background:white;
            padding:40px;
            border-radius:12px;
            box-shadow:0 6px 15px rgba(0,0,0,0.2);
            text-align:center;
        }

        .title{
            font-size:26px;
            color:#b30000;
            margin-bottom:20px;
            font-weight:bold;
        }

        .input{
            width:100%;
            padding:10px;
            margin-top:8px;
            margin-bottom:8px;
            border:1px solid #ccc;
            border-radius:6px;
            box-sizing:border-box;
        }

        .btn{
            width:100%;
            padding:10px;
            background:#b30000;
            color:white;
            border:none;
            border-radius:6px;
            cursor:pointer;
            margin-top:10px;
        }

        .btn:hover{
            background:#ff3333;
        }

        .linkbtn{
            margin-top:15px;
            background:none;
            border:none;
            color:#b30000;
            cursor:pointer;
        }

        .error{
            color:red;
            font-size:13px;
            display:block;
            text-align:left;
            margin-bottom:8px;
        }

        .message{
            color:red;
            display:block;
            margin-top:5px;
            margin-bottom:10px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <div class="card">

            <div class="title">
                Create Account
            </div>

            <asp:Label ID="Label1" runat="server" Text="Full Name"></asp:Label>
            <asp:TextBox ID="txtFullName" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="rfvFullName"
                runat="server"
                ControlToValidate="txtFullName"
                ErrorMessage="Full Name is required."
                CssClass="error"
                Display="Dynamic" />

            <asp:Label ID="Label2" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="txtRegEmail" runat="server" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="rfvEmail"
                runat="server"
                ControlToValidate="txtRegEmail"
                ErrorMessage="Email is required."
                CssClass="error"
                Display="Dynamic" />

            <asp:RegularExpressionValidator
                ID="revEmail"
                runat="server"
                ControlToValidate="txtRegEmail"
                ErrorMessage="Invalid email format."
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                CssClass="error"
                Display="Dynamic" />

            <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txtRegPassword" runat="server" TextMode="Password" CssClass="input"></asp:TextBox>
            <asp:RequiredFieldValidator
                ID="rfvPassword"
                runat="server"
                ControlToValidate="txtRegPassword"
                ErrorMessage="Password is required."
                CssClass="error"
                Display="Dynamic" />

            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="message">
            </asp:Label>

            <asp:Button
                ID="btnRegisterSubmit"
                runat="server"
                Text="Register"
                CssClass="btn"
                OnClick="btnRegisterSubmit_Click" />

            <asp:Button
                ID="btnBackLogin"
                runat="server"
                Text="Back to Login"
                CssClass="linkbtn"
                OnClick="btnBackLogin_Click"
                CausesValidation="false" />

        </div>
    </form>
</body>
</html>