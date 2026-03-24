<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="MandarinQuest.AdminPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>MandarinQuest Admin Dashboard</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f9fafb;
display:flex;
justify-content:center;
align-items:center;
min-height:100vh;
}

/* MAIN CONTAINER */

.container{
width:900px;
}

/* HEADER */

.header{
text-align:center;
margin-bottom:30px;
}

.header h1{
color:#b91c1c;
}

/* DASHBOARD CARD */

.card{
background:white;
padding:35px;
border-radius:12px;
box-shadow:0 6px 15px rgba(0,0,0,0.08);
margin-bottom:25px;
text-align:center;
}

/* MENU GRID */

.menu{
display:grid;
grid-template-columns:repeat(3,1fr);
gap:20px;
}

/* BUTTONS */

.menu input[type=submit]{
padding:15px;
border:none;
background:#dc2626;
color:white;
border-radius:10px;
font-size:15px;
cursor:pointer;
transition:0.2s;
}

.menu input[type=submit]:hover{
background:#b91c1c;
transform:scale(1.05);
}

/* LOGOUT */

.logout{
background:#7f1d1d !important;
}

.logout:hover{
background:#991b1b !important;
}

background:#991b1b !important;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<div class="header">

<h1>MandarinQuest Admin Panel</h1>

<p>System Administration Dashboard</p>

</div>


<div class="card">
<div class="card">

<h2>Admin Controls</h2>

<div class="menu">

<asp:Button ID="btnManageUsers" runat="server"
Text="Manage Users"
OnClick="btnManageUsers_Click" />

<asp:Button ID="btnCreateUsers" runat="server"
Text="Create Users"
OnClick="btnCreateUsers_Click" />

<asp:Button ID="btnViewReports" runat="server"
Text="View Reports"
OnClick="btnViewReports_Click" />

<asp:Button ID="btnAuditLogs" runat="server"
Text="Audit Logs"
OnClick="btnAuditLogs_Click" />

<asp:Button ID="btnTeachingDashboard" runat="server"
Text="Teaching Dashboard"
OnClick="btnTeachingDashboard_Click" />

<asp:Button ID="btnLogoutAdmin" runat="server"
Text="Logout"
CssClass="logout"
OnClick="btnLogoutAdmin_Click" />

</div>

</div>

</div>

</form>

</body>
</html>