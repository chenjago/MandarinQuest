<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProgressTracking.aspx.cs" Inherits="MandarinQuest.ProgressTracking" %>

<!DOCTYPE html>
<html>
<head runat="server">

<title>MandarinQuest Progress</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f4f6f9;
}

/* Header */

.header{
background:#ff4b4b;
color:white;
padding:20px;
font-size:24px;
font-weight:600;
}

/* Container */

.container{
padding:40px;
max-width:1000px;
margin:auto;
}

/* Level Display */

.level{
font-size:18px;
font-weight:600;
margin-bottom:25px;
color:#444;
}

/* Stats Cards */

.stats{
display:flex;
gap:20px;
margin-bottom:30px;
}

.stat-card{
flex:1;
background:white;
padding:20px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.08);
text-align:center;
}

.stat-number{
font-size:28px;
font-weight:bold;
color:#ff4b4b;
}

.stat-label{
font-size:14px;
color:#666;
}

/* Table */

.progress-table{
background:white;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.08);
padding:20px;
}

/* Grid */

.progress-grid{
width:100%;
border-collapse:collapse;
font-size:14px;
}

.progress-grid th{
background:#ff4b4b;
color:white;
padding:12px;
text-align:left;
}

.progress-grid td{
padding:12px;
border-bottom:1px solid #eee;
}

.progress-grid tr:nth-child(even){
background:#fafafa;
}

/* Progress Bar */

.progress-bar{
background:#eee;
height:20px;
border-radius:20px;
overflow:hidden;
margin-top:10px;
}

.progress-fill{
background:#4CAF50;
height:100%;
width:0%;
}

/* Button */

.back-btn{
margin-top:25px;
background:#ff4b4b;
color:white;
border:none;
padding:10px 20px;
border-radius:8px;
cursor:pointer;
}

.back-btn:hover{
background:#e23b3b;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="header">
MandarinQuest — Learning Progress
</div>

<div class="container">

<div class="level">
Current Level: 
<asp:Label ID="lblCurrentLevel" runat="server" ForeColor="#ff4b4b"></asp:Label>
</div>


<!-- Stats -->

<div class="stats">

<div class="stat-card">
<div class="stat-number">
<asp:Label ID="lblCompleted" runat="server"></asp:Label>
</div>
<div class="stat-label">Lessons Completed</div>
</div>

<div class="stat-card">
<div class="stat-number">
<asp:Label ID="lblInProgress" runat="server"></asp:Label>
</div>
<div class="stat-label">Lessons In Progress</div>
</div>

<div class="stat-card">
<div class="stat-number">
<asp:Label ID="lblTotal" runat="server"></asp:Label>
</div>
<div class="stat-label">Total Lessons</div>
</div>

</div>


<!-- Progress Table -->

<div class="progress-table">

<asp:GridView 
ID="dgvProgress"
runat="server"
AutoGenerateColumns="false"
CssClass="progress-grid"
GridLines="None">

<Columns>

<asp:BoundField DataField="LessonTitle" HeaderText="Lesson"/>

<asp:BoundField DataField="Status" HeaderText="Status"/>

<asp:BoundField 
DataField="CompletionDate" 
HeaderText="Completed On" 
DataFormatString="{0:dd MMM yyyy}"/>

</Columns>

</asp:GridView>

</div>


<!-- Overall Progress -->

<div style="margin-top:30px;">

<h3>Overall Progress</h3>

<div class="progress-bar">
<div id="progressFill" runat="server" class="progress-fill"></div>
</div>

<div style="margin-top:8px;font-weight:600;">
<asp:Label ID="lblProgressPercent" runat="server"></asp:Label>
</div>

</div>


<asp:Button
ID="btnBackDashboard"
runat="server"
Text="Back to Dashboard"
CssClass="back-btn"
OnClick="btnBackDashboard_Click"
/>

</div>

</form>

</body>
</html>