<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadMaterials.aspx.cs" Inherits="MandarinQuest.UploadMaterials" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
<title>Upload Materials</title>

<style>

/* TOP NAVBAR */

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

body{
font-family:Segoe UI;
background:#fff5f5;
margin:0;
}

.header{
background:#b30000;
color:white;
padding:20px;
text-align:center;
font-size:24px;
}

.container{
width:900px;
margin:auto;
margin-top:30px;
}

.section{
background:white;
padding:20px;
border-radius:10px;
box-shadow:0px 4px 10px rgba(0,0,0,0.2);
margin-bottom:30px;
}

.section h3{
color:#b30000;
}

input,select{
width:100%;
padding:8px;
margin-top:5px;
margin-bottom:10px;
}

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

.grid th{
background:#b30000;
color:white;
}

.grid td,.grid th{
padding:8px;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<!-- TOP NAVIGATION -->

<div class="navbar">

<a href="TeacherPage.aspx">Dashboard</a>
<a href="ManageLevels.aspx">Manage Levels</a>
<a href="ManageLessons.aspx">Manage Lessons</a>
<a href="UploadMaterials.aspx">Upload Materials</a>
<a href="ScheduleSessions.aspx">Schedule Session</a>
<a href="ViewParticipation.aspx">Participation</a>

</div>

<div class="header">
Teacher – Upload Materials
</div>

<div class="container">

<div class="section">

<h3>Upload Material</h3>

Material Name

<asp:TextBox ID="txtMaterialName" runat="server"></asp:TextBox>

Material Type

<asp:DropDownList ID="ddlMaterialType" runat="server">
<asp:ListItem Text="PDF" Value="PDF"/>
<asp:ListItem Text="Audio" Value="Audio"/>
<asp:ListItem Text="Video" Value="Video"/>
<asp:ListItem Text="Image" Value="Image"/>
</asp:DropDownList>

Select Lesson (Optional)

<asp:DropDownList ID="ddlLessons" runat="server">
<asp:ListItem Text="General Resource (No Lesson)" Value="0"/>
</asp:DropDownList>

File

<asp:FileUpload ID="fileUpload" runat="server" />

<br />

<asp:Button
ID="btnUpload"
runat="server"
Text="Upload Material"
CssClass="btn"
OnClick="btnUpload_Click"/>

</div>


<div class="section">

<h3>Uploaded Materials</h3>

<asp:GridView
ID="gvMaterials"
runat="server"
CssClass="grid"
AutoGenerateColumns="False"
OnRowCommand="gvMaterials_RowCommand">

<Columns>

<asp:BoundField DataField="MaterialID" HeaderText="ID" />

<asp:BoundField DataField="FileName" HeaderText="File Name" />

<asp:BoundField DataField="FilePath" HeaderText="Path" />

<asp:BoundField DataField="UploadDate" HeaderText="Upload Date" />

<asp:TemplateField HeaderText="Action">

<ItemTemplate>

<asp:Button
ID="btnDelete"
runat="server"
Text="Delete"
CssClass="btn"
CommandName="deleteMaterial"
CommandArgument='<%# Eval("MaterialID") %>'
OnClientClick="return confirm('Delete this material?');"
/>

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>