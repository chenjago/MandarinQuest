<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LearningMaterials.aspx.cs" Inherits="MandarinQuest.LearningMaterials" %>

<!DOCTYPE html>
<html>
<head runat="server">
<title>Learning Materials</title>

<style>

body{
font-family:'Segoe UI';
background:#f6efe6;
margin:0;
}

.container{
padding:40px;
}

h2{
color:#a11212;
margin-bottom:20px;
}

.searchBar{
margin-bottom:20px;
}

.searchBar input{
padding:8px;
width:260px;
}

.btnSearch{
background:#a11212;
color:white;
border:none;
padding:8px 14px;
border-radius:5px;
cursor:pointer;
}

.materialCard{
background:white;
padding:18px;
margin-bottom:12px;
border-radius:8px;
box-shadow:0 3px 8px rgba(0,0,0,0.15);
display:flex;
justify-content:space-between;
align-items:center;
}

.file{
font-weight:bold;
}

.type{
font-size:12px;
color:#777;
}

.btn{
background:#a11212;
color:white;
border:none;
padding:6px 12px;
border-radius:5px;
cursor:pointer;
margin-left:6px;
}

.btn:hover{
background:#c71d1d;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h2>Learning Materials</h2>

<div class="searchBar">

<asp:TextBox 
ID="txtSearch"
runat="server"
placeholder="Search material name..." />

<asp:Button
ID="btnSearch"
runat="server"
Text="Search"
CssClass="btnSearch"
OnClick="btnSearch_Click"/>

</div>

<asp:Repeater ID="rptMaterials" runat="server">

<ItemTemplate>

<div class="materialCard">

<div>

<div class="file">
<%# Eval("FileName") %>
</div>

<div class="type">
Type: <%# Eval("MaterialType") %>
</div>

</div>

<div>

<asp:Button
runat="server"
Text="View"
CssClass="btn"
CommandArgument='<%# Eval("FilePath") %>'
OnCommand="ViewMaterial"/>

<asp:Button
runat="server"
Text="Download"
CssClass="btn"
CommandArgument='<%# Eval("FilePath") %>'
OnCommand="DownloadMaterial"/>

</div>

</div>

</ItemTemplate>

</asp:Repeater>

</div>

</form>

</body>
</html>