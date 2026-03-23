<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageLevels.aspx.cs" Inherits="MandarinQuest.ManageLevels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

<title>Manage Levels</title>

<style>

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
font-family:'Segoe UI';
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

.topbar{
text-align:left;
margin-bottom:20px;
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
}

.section{
background:white;
padding:25px;
margin-bottom:30px;
border-radius:10px;
box-shadow:0px 4px 12px rgba(0,0,0,0.15);
}

.section h3{
color:#b30000;
margin-top:0;
}

input, textarea{
width:100%;
padding:8px;
margin-top:5px;
margin-bottom:12px;
border-radius:5px;
border:1px solid #ccc;
}

.btn{
background:#b30000;
color:white;
border:none;
padding:10px 15px;
border-radius:6px;
cursor:pointer;
}

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

.deleteBtn{
background:#e53935;
color:white;
border:none;
padding:6px 10px;
border-radius:4px;
cursor:pointer;
}

.modal{
display:none;
position:fixed;
top:0;
left:0;
width:100%;
height:100%;
background:rgba(0,0,0,0.5);
}

.modal-content{
background:white;
width:420px;
margin:200px auto;
padding:25px;
border-radius:10px;
}

.toast{
visibility:hidden;
min-width:250px;
background:#4CAF50;
color:white;
text-align:center;
border-radius:5px;
padding:14px;
position:fixed;
right:30px;
top:30px;
}

.toast.show{
visibility:visible;
}

</style>

<script>

    function showDeleteModal(levelID) {

        document.getElementById("hiddenLevelID").value = levelID;
        document.getElementById("deleteModal").style.display = "block";

    }

    function closeModal() {

        document.getElementById("deleteModal").style.display = "none";

    }

    function showToast(message) {

        var toast = document.getElementById("toast");

        toast.innerHTML = message;
        toast.className = "toast show";

        setTimeout(function () {
            toast.className = toast.className.replace("show", "");
        }, 3000);

    }

</script>

</head>

<body>

<form id="form1" runat="server">

<div class="navbar">

<a href="TeacherPage.aspx">Dashboard</a> <a href="ManageLevels.aspx">Manage Levels</a> <a href="ManageLessons.aspx">Manage Lessons</a> <a href="UploadMaterials.aspx">Upload Materials</a> <a href="ScheduleSessions.aspx">Schedule Session</a> <a href="ViewParticipation.aspx">Participation</a>

</div>

<div class="header">
Teacher – Manage Learning Levels
</div>

<div class="container">

<div class="topbar">

<asp:Button
ID="btnBack"
runat="server"
Text="← Back"
CssClass="backbtn"
OnClick="btnBack_Click"/>

</div>

<div class="section">

<h3>Create Level</h3>

Level Name

<asp:TextBox ID="txtLevelName" runat="server"></asp:TextBox>

Description

<asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine"></asp:TextBox>

Level Order

<asp:TextBox ID="txtLevelOrder" runat="server"></asp:TextBox>

<asp:Button
ID="btnAddLevel"
runat="server"
Text="Add Level"
CssClass="btn"
OnClick="btnAddLevel_Click"/>

</div>

<div class="section">

<h3>Available Levels</h3>

<asp:GridView
ID="dgvLevels"
runat="server"
CssClass="grid"
AutoGenerateColumns="false">

<Columns>

<asp:BoundField DataField="LevelID" HeaderText="ID"/>
<asp:BoundField DataField="LevelName" HeaderText="Level"/>
<asp:BoundField DataField="Description" HeaderText="Description"/>
<asp:BoundField DataField="LevelOrder" HeaderText="Order"/>
<asp:BoundField DataField="Status" HeaderText="Status"/>

<asp:TemplateField HeaderText="Action">

<ItemTemplate>

<button type="button" class="deleteBtn"
onclick='showDeleteModal(<%# Eval("LevelID") %>)'>
Delete </button>

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

<div id="deleteModal" class="modal">

<div class="modal-content">

<h3>Delete Level</h3>

Reason

<asp:TextBox ID="txtDeleteReason" runat="server" TextMode="MultiLine"></asp:TextBox>

<asp:HiddenField ID="hiddenLevelID" runat="server"/>

<br /><br />

<asp:Button
ID="btnConfirmDelete"
runat="server"
Text="Confirm Delete"
CssClass="btn"
OnClick="btnConfirmDelete_Click"/>

<button type="button" onclick="closeModal()">Cancel</button>

</div>

</div>

<div id="toast" class="toast"></div>

</form>

</body>
</html>
