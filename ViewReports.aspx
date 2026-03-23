<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewReports.aspx.cs" Inherits="MandarinQuest.ViewReports" %>

<!DOCTYPE html>
<html>
<head runat="server">
<title>System Reports</title>

<style>
body{
font-family:'Segoe UI';
background:#f6efe6;
}

.container{
padding:40px;
}

.card{
background:white;
padding:20px;
margin:10px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.15);
}

</style>

</head>

<body>

<form runat="server">

<div class="container">

<div class="card">
Total Users:
<asp:Label ID="lblUsers" runat="server"></asp:Label>
</div>

<div class="card">
Total Lessons:
<asp:Label ID="lblLessons" runat="server"></asp:Label>
</div>

<div class="card">
Total Materials:
<asp:Label ID="lblMaterials" runat="server"></asp:Label>
</div>

</div>

</form>

</body>
</html>