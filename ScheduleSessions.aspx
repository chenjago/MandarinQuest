<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleSessions.aspx.cs" Inherits="MandarinQuest.ScheduleSessions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<title>Schedule Session</title>

<style>

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

/* BODY */

body{
font-family:'Segoe UI';
background:#fff5f5;
margin:0;
}

/* HEADER */

.header{
background:#b30000;
color:white;
padding:20px;
text-align:center;
font-size:24px;
}

/* CONTAINER */

.container{
width:900px;
margin:auto;
margin-top:30px;
}

/* SECTIONS */

.section{
background:white;
padding:20px;
border-radius:10px;
box-shadow:0px 4px 10px rgba(0,0,0,0.2);
margin-bottom:30px;
}

.section h3{
color:#b30000;
margin-top:0;
}

/* INPUTS */

input,select{
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

/* GRID */

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

<div class="header">
Teacher – Schedule Session
</div>

<div class="container">

<!-- CREATE SESSION -->

<div class="section">

<h3>Create Session</h3>

Select Level

<asp:DropDownList ID="ddlClasses" runat="server"></asp:DropDownList>

Session Title

<asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>

Session Date

<asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox>

Session Time

<asp:TextBox ID="txtTime" runat="server" TextMode="Time"></asp:TextBox>

Meeting Link

<asp:TextBox ID="txtLink" runat="server"></asp:TextBox>

<asp:Button
ID="btnCreateSession"
runat="server"
Text="Schedule Session"
CssClass="btn"
OnClick="btnCreateSession_Click"/>

</div>

<!-- SESSION TABLE -->

<div class="section">

<h3>Scheduled Sessions</h3>

<asp:GridView
ID="gvSessions"
runat="server"
CssClass="grid"
AutoGenerateColumns="false"
OnRowCommand="gvSessions_RowCommand">

<Columns>

<asp:BoundField DataField="LevelName" HeaderText="Level"/>
<asp:BoundField DataField="SessionTitle" HeaderText="Session"/>
<asp:BoundField DataField="SessionDate" HeaderText="Date"/>
<asp:BoundField DataField="SessionLink" HeaderText="Meeting Link"/>

<asp:TemplateField HeaderText="Action">

<ItemTemplate>

<asp:Button
runat="server"
Text="Delete"
CssClass="deleteBtn"
CommandName="DeleteSession"
CommandArgument='<%# Eval("SessionID") %>'/>

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>
