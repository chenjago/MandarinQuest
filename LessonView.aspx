<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LessonView.aspx.cs" Inherits="MandarinQuest.LessonView" %>

<!DOCTYPE html>

<html>
<head runat="server">
<title>Lessons</title>

<style>

body{
font-family:'Segoe UI';
background:#f5f5f5;
margin:0;
}

.container{
padding:40px;
}

h2{
margin-bottom:25px;
}

.lesson-card{
background:white;
padding:20px;
border-radius:10px;
margin-bottom:15px;
box-shadow:0 4px 8px rgba(0,0,0,0.1);
display:flex;
justify-content:space-between;
align-items:center;
}

.btn{
padding:8px 16px;
border:none;
border-radius:6px;
cursor:pointer;
margin-left:8px;
}

.open{
background:#3498db;
color:white;
}

.done{
background:#27ae60;
color:white;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h2>Lessons</h2>

<asp:Repeater ID="rptLessons" runat="server">

<ItemTemplate>

<div class="lesson-card">

<div>
<b><%# Eval("LessonTitle") %></b>
</div>

<div>

<asp:Button
ID="btnOpen"
runat="server"
Text="Open Lesson"
CssClass="btn open"
CommandName="open"
CommandArgument='<%# Eval("LessonID") %>'
OnCommand="OpenLesson"
/>

<asp:Button
ID="btnDone"
runat="server"
Text="Done"
CssClass="btn done"
CommandArgument='<%# Eval("LessonID") %>'
OnCommand="CompleteLesson"
/>

</div>

</div>

</ItemTemplate>

</asp:Repeater>

</div>

</form>

</body>
</html>