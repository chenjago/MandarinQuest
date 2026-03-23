<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewParticipation.aspx.cs" Inherits="MandarinQuest.ViewParticipation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<title>Participation</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f6f6f6;
}

/* NAVBAR */

.navbar{
background:#b30000;
padding:15px;
text-align:center;
}

.navbar a{
color:white;
margin:0 15px;
text-decoration:none;
font-weight:bold;
}

.navbar a:hover{
text-decoration:underline;
}

/* HEADER */

.header{
font-size:28px;
font-weight:bold;
color:#b30000;
text-align:center;
margin-top:30px;
}

/* CONTAINER */

.container{
width:1000px;
margin:auto;
padding:30px;
}

/* FILTER SECTION */

.section{
background:white;
padding:20px;
border-radius:10px;
box-shadow:0 4px 8px rgba(0,0,0,0.1);
margin-bottom:30px;
}

/* INPUT */

select{
width:100%;
padding:8px;
margin-top:5px;
margin-bottom:10px;
}

/* TABLE */

.grid{
width:100%;
border-collapse:collapse;
}

.grid th{
background:#b30000;
color:white;
padding:10px;
}

.grid td{
padding:10px;
border-bottom:1px solid #ddd;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<!-- NAVBAR -->

<div class="navbar">

<a href="TeacherPage.aspx">Dashboard</a> <a href="ManageLevels.aspx">Manage Levels</a> <a href="ManageLessons.aspx">Manage Lessons</a> <a href="UploadMaterials.aspx">Upload Materials</a> <a href="ScheduleSessions.aspx">Schedule Session</a> <a href="ViewParticipation.aspx">Participation</a>

</div>

<div class="container">

<div class="header">
Student Participation
</div>

<div class="section">

<h3>Filter</h3>

Level

<asp:DropDownList
ID="ddlLevels"
runat="server"
AutoPostBack="true"
OnSelectedIndexChanged="ddlLevels_SelectedIndexChanged">
</asp:DropDownList>

Lesson

<asp:DropDownList
ID="ddlLessons"
runat="server"
AutoPostBack="true"
OnSelectedIndexChanged="ddlLessons_SelectedIndexChanged">
</asp:DropDownList>

</div>

<div class="section">

<h3>Participation Records</h3>

<asp:GridView
ID="dgvParticipation"
runat="server"
CssClass="grid"
AutoGenerateColumns="false">

<Columns>

<asp:BoundField DataField="Fullname" HeaderText="Student"/>
<asp:BoundField DataField="LevelName" HeaderText="Level"/>
<asp:BoundField DataField="LessonTitle" HeaderText="Lesson"/>
<asp:BoundField DataField="CompletionStatus" HeaderText="Status"/>
<asp:BoundField DataField="CompletionDate" HeaderText="Completed On"/>

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>
