<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LessonMaterials.aspx.cs" Inherits="MandarinQuest.LessonMaterials" %>

<!DOCTYPE html>
<html>
<head runat="server">

<title>Lesson Materials</title>

<style>

body{
font-family:'Segoe UI';
background:#f5f5f5;
margin:0;
}

.header{
background:#b30000;
color:white;
padding:18px;
text-align:center;
font-size:22px;
}

.container{
width:900px;
margin:auto;
margin-top:30px;
}

.material-card{
background:white;
padding:18px;
border-radius:10px;
margin-bottom:15px;
box-shadow:0 4px 10px rgba(0,0,0,0.15);
display:flex;
justify-content:space-between;
align-items:center;
}

.material-info{
flex:1;
}

.material-title{
font-weight:bold;
font-size:18px;
}

.material-type{
color:#777;
font-size:14px;
}

.btn{
background:#b30000;
color:white;
border:none;
padding:8px 14px;
border-radius:6px;
cursor:pointer;
margin-left:8px;
}

.btn:hover{
background:#ff3333;
}

.backBtn{
margin-top:25px;
background:#555;
}

.noMaterial{
text-align:center;
color:#777;
margin-top:20px;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="header">
Lesson Materials
</div>

<div class="container">

<asp:Repeater ID="rptMaterials" runat="server">

<ItemTemplate>

<div class="material-card">

<div class="material-info">

<div class="material-title">
<%# Eval("FileName") %>
</div>

<div class="material-type">
Type: <%# Eval("MaterialType") %>
</div>

</div>

<div>

<asp:Button
runat="server"
Text="View"
CssClass="btn"
CommandArgument='<%# Eval("FilePath") %>'
OnCommand="ViewMaterial"
/>

<asp:Button
runat="server"
Text="Download"
CssClass="btn"
CommandArgument='<%# Eval("FilePath") %>'
OnCommand="DownloadMaterial"
/>

</div>

</div>

</ItemTemplate>

</asp:Repeater>


<asp:Label 
ID="lblNoMaterial"
runat="server"
Text="No materials available for this lesson."
CssClass="noMaterial"
Visible="false"/>


<br />

<asp:Button
ID="btnBack"
runat="server"
Text="Back"
CssClass="btn backBtn"
OnClick="btnBack_Click"/>

</div>

</form>

</body>
</html>