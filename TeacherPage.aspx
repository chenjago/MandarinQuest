<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeacherPage.aspx.cs" Inherits="MandarinQuest.TeacherPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>Teacher Dashboard</title>

<style>

/* PAGE */

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

/* CONTAINER */

.container{
width:1000px;
margin:auto;
padding:30px;
}

/* HEADER */

.header{
font-size:28px;
font-weight:bold;
color:#b30000;
text-align:center;
}

.welcome{
text-align:center;
margin-top:10px;
color:#666;
}

/* STATS */

.stats{
display:flex;
gap:25px;
margin-top:40px;
}

.statcard{
flex:1;
background:white;
padding:20px;
border-radius:10px;
box-shadow:0 4px 8px rgba(0,0,0,0.1);
text-align:center;
}

.statcard h2{
margin:0;
color:#b30000;
}

/* ACTION CARDS */

.actions{
margin-top:40px;
display:grid;
grid-template-columns:repeat(2,1fr);
gap:25px;
}

.card{
background:white;
padding:25px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.15);
text-align:center;
transition:0.3s;
}

.card:hover{
transform:translateY(-5px);
}

.card h3{
margin-bottom:10px;
}

/* BUTTON */

.btn{
background:#b30000;
color:white;
border:none;
padding:10px 15px;
border-radius:6px;
cursor:pointer;
}

.btn:hover{
background:#ff3333;
}

/* SESSION AREA */

.sessions{
margin-top:40px;
}

.sessionTitle{
font-size:20px;
margin-bottom:20px;
color:#b30000;
}

/* SESSION CARDS */

.sessionGrid{
display:grid;
grid-template-columns:repeat(auto-fill,minmax(250px,1fr));
gap:20px;
}

.sessionCard{
background:white;
padding:20px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.15);
}

.sessionLevel{
font-size:14px;
color:#999;
}

.sessionName{
font-size:18px;
font-weight:bold;
margin-top:5px;
}

.sessionDate{
margin-top:10px;
color:#555;
}

.joinBtn{
margin-top:15px;
display:inline-block;
background:#b30000;
color:white;
padding:8px 14px;
border-radius:5px;
text-decoration:none;
}

.joinBtn:hover{
background:#ff3333;
}

.noSession{
text-align:center;
color:#777;
margin-top:20px;
}

/* FOOTER */

.footer{
margin-top:40px;
text-align:center;
color:#888;
font-size:14px;
}

.logout{
text-align:center;
margin-top:30px;
}

</style>

    <script>

function copyLink(link)
{
    navigator.clipboard.writeText(link).then(function(){
        alert("Session link copied to clipboard!");
    });
}

</script>

</head>

<body>

<form id="form1" runat="server">

<!-- NAVBAR -->

<div class="navbar">

<a href="TeacherPage.aspx">Dashboard</a>
<a href="ManageLevels.aspx">Manage Levels</a>
<a href="ManageLessons.aspx">Manage Lessons</a>
<a href="UploadMaterials.aspx">Upload Materials</a>
<a href="ScheduleSessions.aspx">Schedule Session</a>
<a href="ViewParticipation.aspx">Participation</a>

</div>


<div class="container">

<div class="header">
MandarinQuest Teacher Dashboard
</div>

<div class="welcome">
Welcome,
<asp:Label ID="lblWelcomeTeacher" runat="server"></asp:Label>
</div>


<!-- STATS -->

<div class="stats">

<div class="statcard">
<h2><asp:Label ID="lblClassCount" runat="server"></asp:Label></h2>
<p>Levels</p>
</div>

<div class="statcard">
<h2><asp:Label ID="lblLessonCount" runat="server"></asp:Label></h2>
<p>Lessons</p>
</div>

<div class="statcard">
<h2><asp:Label ID="lblStudentCount" runat="server"></asp:Label></h2>
<p>Students</p>
</div>

</div>


<!-- ACTION CARDS -->

<div class="actions">

<div class="card">
<h3>📚 Manage Levels</h3>
<p>Create and organize learning levels</p>

<asp:Button
ID="btnManageLevels"
runat="server"
Text="Open"
CssClass="btn"
OnClick="btnManageLevels_Click"/>
</div>

<div class="card">
<h3>📖 Manage Lessons</h3>
<p>Create lessons for each level</p>

<asp:Button
ID="btnManageLessons"
runat="server"
Text="Open"
CssClass="btn"
OnClick="btnManageLessons_Click"/>
</div>

<div class="card">
<h3>📂 Upload Materials</h3>
<p>Upload PDFs, videos and audio</p>

<asp:Button
ID="btnUploadMaterials"
runat="server"
Text="Upload"
CssClass="btn"
OnClick="btnUploadMaterials_Click"/>
</div>

<div class="card">
<h3>📅 Schedule Sessions</h3>
<p>Plan upcoming sessions</p>

<asp:Button
ID="btnScheduleSessions"
runat="server"
Text="Schedule"
CssClass="btn"
OnClick="btnScheduleSessions_Click"/>
</div>

</div>


<!-- UPCOMING SESSIONS -->

<div class="sessions">

<div class="sessionTitle">
Upcoming Sessions
</div>

<asp:Repeater ID="rptSessions" runat="server">

<ItemTemplate>

<div class="sessionGrid">

<div class="sessionCard">

<div class="sessionLevel">
Level: <%# Eval("LevelName") %>
</div>

<div class="sessionName">
<%# Eval("SessionTitle") %>
</div>

<div class="sessionDate">
Date: <%# Eval("SessionDate","{0:dd MMM yyyy}") %>
</div>

<button 
type="button"
class="joinBtn"
onclick="copyLink('<%# Eval("SessionLink") %>')">
Copy Link
</button>

</div>

</div>

</ItemTemplate>

</asp:Repeater>

<asp:Label 
ID="lblNoSessions" 
runat="server"
Text="No upcoming sessions scheduled."
CssClass="noSession"
Visible="false"/>

</div>


<div class="logout">

<asp:Button
ID="btnLogoutInstructor"
runat="server"
Text="Logout"
CssClass="btn"
OnClick="btnLogoutInstructor_Click"/>

</div>

<div class="footer">
MandarinQuest Learning System © 2026
</div>

</div>

</form>

</body>
</html>