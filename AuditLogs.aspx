<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLogs.aspx.cs" Inherits="MandarinQuest.AuditLogs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>Audit Logs</title>

<style>

body{
font-family:'Segoe UI';
background:#f9fafb;
padding:40px;
}

.container{
max-width:800px;
margin:auto;
}

h1{
text-align:center;
color:#b91c1c;
margin-bottom:25px;
}

.card{
background:white;
padding:25px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.08);
}

.log{
padding:10px;
border-bottom:1px solid #eee;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h1>Audit Logs</h1>

<div class="card">

<asp:Literal ID="litLogs" runat="server"></asp:Literal>

</div>

</div>

</form>

</body>
</html>