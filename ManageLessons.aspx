<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLessons.aspx.cs" Inherits="MandarinQuest.ManageLessons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<title>Manage Lessons</title>

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

/* BACK BUTTON */

.topbar{
padding:20px;
}

.backbtn{
background:#444;
color:white;
border:none;
width:60px;
height:40px;
font-size:12px;
cursor:pointer;
border-radius:6px;
display:inline-flex;
align-items:center;
justify-content:center;
}

/* CONTAINER */

.container{
width:1000px;
margin:auto;
}

/* SECTIONS */

.section{
background:white;
padding:20px;
margin-bottom:30px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.1);
}

.section h3{
color:#b30000;
margin-top:0;
}

/* INPUTS */

input,textarea,select{
width:100%;
padding:8px;
margin-top:5px;
margin-bottom:10px;
}

/* BUTTON */

.btn{
background:#b30000;
color:white;
border:none;
padding:10px 15px;
border-radius:5px;
cursor:pointer;
}

.btn:hover{
background:#ff3333;
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

/* DELETE BUTTON */

.deleteBtn{
background:#e53935;
color:white;
border:none;
padding:6px 10px;
border-radius:4px;
cursor:pointer;
}

.deleteBtn:hover{
background:#b71c1c;
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

<!-- BACK BUTTON -->

<div class="topbar">

<asp:Button
ID="btnBack"
runat="server"
Text="⬅ Back"
CssClass="backbtn"
OnClick="btnBack_Click"/>

</div>

<!-- LEVEL FILTER -->

<div class="section">

<h3>Filter by Level</h3>

<asp:DropDownList
ID="ddlClasses"
runat="server"
AutoPostBack="true"
OnSelectedIndexChanged="ddlClasses_SelectedIndexChanged">
</asp:DropDownList>

</div>

<!-- CREATE LESSON -->

<div class="section">

<h3>Create Lesson</h3>

Lesson Title

<asp:TextBox ID="txtLessonTitle" runat="server"></asp:TextBox>

Description

<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>

<asp:Button
ID="btnAddLesson"
runat="server"
Text="Add Lesson"
CssClass="btn"
OnClick="btnAddLesson_Click"/>

</div>

<!-- LESSON TABLE -->

<div class="section">

<h3>Lessons</h3>

<asp:GridView
ID="gvLessons"
runat="server"
CssClass="grid"
AutoGenerateColumns="false"
OnRowCommand="gvLessons_RowCommand">

<Columns>

<asp:BoundField DataField="LessonID" HeaderText="ID"/>
<asp:BoundField DataField="LevelName" HeaderText="Level"/>
<asp:BoundField DataField="LessonTitle" HeaderText="Lesson"/>
<asp:BoundField DataField="Description" HeaderText="Description"/>
<asp:BoundField DataField="CreatedDate" HeaderText="Created"/>

<asp:TemplateField HeaderText="Delete">

<ItemTemplate>

<asp:Button
runat="server"
Text="Delete"
CssClass="deleteBtn"
CommandName="DeleteLesson"
CommandArgument='<%# Eval("LessonID") %>'/>

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>
