<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="MandarinQuest.AdminPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>MandarinQuest Admin Dashboard</title>

<style>

/* GLOBAL */

body{
    margin:0;
    font-family:'Segoe UI', sans-serif;
    background:#f9fafb;
    display:flex;
    justify-content:center;
    align-items:center;
    min-height:100vh;
}

/* CONTAINER */

.container{
    max-width:900px;
    width:90%;
}

/* HEADER */

.header{
    text-align:center;
    margin-bottom:30px;
}

.header h1{
    color:#b91c1c;
    margin-bottom:5px;
}

.header p{
    color:#555;
}

/* CARD */

.card{
    background:white;
    padding:35px;
    border-radius:12px;
    box-shadow:0 6px 15px rgba(0,0,0,0.08);
    text-align:center;
}

/* MENU GRID */

.menu{
    display:grid;
    grid-template-columns:repeat(2,1fr);
    gap:20px;
    margin-top:20px;
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

/* LOGOUT BUTTON */

.logout{
    margin-top:25px;
    width:100%;
    padding:15px;
    border:none;
    background:#7f1d1d;
    color:white;
    border-radius:10px;
    font-size:15px;
    cursor:pointer;
    transition:0.2s;
}

.logout:hover{
    background:#991b1b;
}

/* RESPONSIVE */

@media (max-width: 768px){
    .menu{
        grid-template-columns:1fr;
    }
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

        <h2>Admin Controls</h2>

        <div class="menu">

            <asp:Button ID="btnManageUsers" runat="server"
            Text="👤 Manage Users"
            ToolTip="View and manage user accounts"
            OnClick="btnManageUsers_Click" />

            <asp:Button ID="btnManageRoles" runat="server"
            Text="🔐 Manage Roles"
            ToolTip="Assign and manage roles"
            OnClick="btnManageRoles_Click" />

            <asp:Button ID="btnViewReports" runat="server"
            Text="📊 View Reports"
            ToolTip="View system reports"
            OnClick="btnViewReports_Click" />

            <asp:Button ID="btnAuditLogs" runat="server"
            Text="📜 Audit Logs"
            ToolTip="Check system activity logs"
            OnClick="btnAuditLogs_Click" />

        </div>

        <!-- Logout separate -->
        <asp:Button ID="btnLogoutAdmin" runat="server"
        Text="🚪 Logout"
        CssClass="logout"
        OnClick="btnLogoutAdmin_Click" />

    </div>

</div>

</form>

</body>
</html>