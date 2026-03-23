<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MandarinQuest.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

<title>MandarinQuest Login</title>

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

/* LOGIN CARD */

.card{
width:350px;
background:white;
padding:40px;
border-radius:12px;
box-shadow:0 6px 15px rgba(0,0,0,0.2);
text-align:center;
}

/* TITLE */

.title{
font-size:26px;
color:#b30000;
margin-bottom:20px;
font-weight:bold;
}

/* INPUT */

.input{
width:100%;
padding:10px;
margin-top:8px;
margin-bottom:15px;
border:1px solid #ccc;
border-radius:6px;
}

/* BUTTON */

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

/* LINK BUTTON */

.linkbtn{
margin-top:15px;
background:none;
border:none;
color:#b30000;
cursor:pointer;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="card">

<div class="title">
MandarinQuest
</div>

<p>Login to your learning portal</p>

<asp:Label ID="lblEmail" runat="server" Text="Email"></asp:Label>

<asp:TextBox 
ID="txtEmail"
runat="server"
CssClass="input">
</asp:TextBox>


<asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label>

<asp:TextBox 
ID="txtPassword"
runat="server"
TextMode="Password"
CssClass="input">
</asp:TextBox>


<asp:Button
ID="btnLoginSubmit"
runat="server"
Text="Login"
CssClass="btn"
OnClick="btnLoginSubmit_Click"/>


<asp:Button
ID="btnGoRegister"
runat="server"
Text="Create Account"
CssClass="linkbtn"
OnClick="btnGoRegister_Click"/>

</div>

</form>

</body>
</html>