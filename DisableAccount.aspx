<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisableAccount.aspx.cs" Inherits="MandarinQuest.DisableAccount" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Disable User Account</title>

<style>

body{
font-family:'Segoe UI';
background:#f4f6f9;
padding:40px;
}

.container{
width:400px;
margin:auto;
background:white;
padding:30px;
border-radius:10px;
box-shadow:0 3px 10px rgba(0,0,0,0.1);
}

input[type=text]{
width:100%;
padding:10px;
margin-top:10px;
border:1px solid #ccc;
border-radius:6px;
}

button{
margin-top:15px;
padding:10px;
width:100%;
background:#dc2626;
color:white;
border:none;
border-radius:6px;
cursor:pointer;
}

button:hover{
background:#b91c1c;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h2>Disable User Account</h2>

Enter User Email or Username

<asp:TextBox ID="txtUser" runat="server"></asp:TextBox>

<br /><br />

<asp:Button ID="btnDisable" runat="server"
Text="Disable Account"
OnClick="btnDisable_Click" />

<br /><br />

<asp:Label ID="lblMessage" runat="server"></asp:Label>

</div>

</form>

</body>
</html>