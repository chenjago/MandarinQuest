<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageRoles.aspx.cs" Inherits="MandarinQuest.ManageRoles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>Manage Roles</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f9fafb;
padding:40px;
}

/* CONTAINER */

.container{
max-width:800px;
margin:auto;
}

/* HEADER */

.header{
text-align:center;
margin-bottom:25px;
}

.header h1{
color:#b91c1c;
}

/* CARD */

.card{
background:white;
padding:25px;
border-radius:12px;
box-shadow:0 4px 10px rgba(0,0,0,0.08);
}

/* INPUT AREA */

.addRole{
margin-bottom:20px;
}

.addRole input{
padding:8px;
width:200px;
border-radius:6px;
border:1px solid #ccc;
}

/* BUTTON */

.btn{
padding:8px 14px;
border:none;
background:#dc2626;
color:white;
border-radius:6px;
cursor:pointer;
}

.btn:hover{
background:#b91c1c;
}

/* GRID */

table{
width:100%;
border-collapse:collapse;
}

th{
background:#b91c1c;
color:white;
padding:10px;
}

td{
padding:10px;
border-bottom:1px solid #eee;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<div class="header">
<h1>Manage Roles</h1>
</div>

<div class="card">

<div class="addRole">

New Role:

<asp:TextBox ID="txtRole" runat="server"></asp:TextBox>

<asp:Button 
ID="btnAddRole"
runat="server"
Text="Add Role"
CssClass="btn"
OnClick="btnAddRole_Click" />

</div>


<asp:GridView
ID="gvRoles"
runat="server"
AutoGenerateColumns="False"
DataKeyNames="RoleID"
OnRowEditing="gvRoles_RowEditing"
OnRowCancelingEdit="gvRoles_RowCancelingEdit"
OnRowUpdating="gvRoles_RowUpdating"
OnRowDeleting="gvRoles_RowDeleting">

<Columns>

<asp:BoundField DataField="RoleID" HeaderText="Role ID" ReadOnly="True" />

<asp:BoundField DataField="RoleName" HeaderText="Role Name" />

<asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>