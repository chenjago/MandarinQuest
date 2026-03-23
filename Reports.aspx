<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="MandarinQuest.Reports" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>System Reports</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f9fafb;
padding:40px;
}

/* CONTAINER */

.container{
max-width:900px;
margin:auto;
}

/* HEADER */

.header{
text-align:center;
margin-bottom:30px;
}

.header h1{
color:#b91c1c;
}

/* CARD */

.card{
background:white;
padding:30px;
border-radius:12px;
box-shadow:0 6px 15px rgba(0,0,0,0.08);
}

/* REPORT GRID */

.reports{
display:grid;
grid-template-columns:repeat(3,1fr);
gap:20px;
margin-top:20px;
}

/* REPORT BOX */

.reportBox{
background:#fff;
border-left:5px solid #dc2626;
padding:20px;
border-radius:8px;
box-shadow:0 3px 8px rgba(0,0,0,0.05);
text-align:center;
}

.reportBox h2{
margin:0;
font-size:28px;
color:#b91c1c;
}

.reportBox p{
margin-top:8px;
color:#555;
}

/* STATUS */

.status{
margin-top:30px;
text-align:center;
font-size:16px;
color:#16a34a;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<div class="header">
<h1>System Reports</h1>
<p>MandarinQuest Administration Statistics</p>
</div>

<div class="card">

<div class="reports">

<div class="reportBox">
<h2><asp:Label ID="lblUsers" runat="server" /></h2>
<p>Total Users</p>
</div>

<div class="reportBox">
<h2><asp:Label ID="lblRoles" runat="server" /></h2>
<p>Total Roles</p>
</div>

<div class="reportBox">
<h2><asp:Label ID="lblLessons" runat="server" /></h2>
<p>Total Lessons</p>
</div>

<div class="reportBox">
<h2><asp:Label ID="lblClasses" runat="server" /></h2>
<p>Total Classes</p>
</div>

<div class="reportBox">
<h2><asp:Label ID="lblStudents" runat="server" /></h2>
<p>Total Students</p>
</div>

<div class="reportBox">
<h2><asp:Label ID="lblTeachers" runat="server" /></h2>
<p>Total Teachers</p>
</div>

</div>

<div class="status">

System Status:
<asp:Label ID="lblStatus" runat="server"></asp:Label>

</div>

</div>

</div>

</form>

</body>
</html>