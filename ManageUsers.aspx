<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="MandarinQuest.ManageUsers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>Manage Users</title>

<style>

body{
font-family:'Segoe UI';
background:#f9fafb;
padding:40px;
}

.container{
max-width:900px;
margin:auto;
}

h1{
text-align:center;
color:#b91c1c;
margin-bottom:25px;
}

.card{
background:white;
padding:25px;
border-radius:10px;
box-shadow:0 4px 10px rgba(0,0,0,0.08);
}

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

.btn{
padding:6px 10px;
border:none;
border-radius:6px;
background:#dc2626;
color:white;
cursor:pointer;
}

.btn:hover{
background:#b91c1c;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h1>Manage Users</h1>

<div class="card">

<asp:GridView
ID="gvUsers"
runat="server"
AutoGenerateColumns="False"
DataKeyNames="UserID"
OnRowEditing="gvUsers_RowEditing"
OnRowUpdating="gvUsers_RowUpdating"
OnRowCancelingEdit="gvUsers_RowCancelingEdit"
OnRowDeleting="gvUsers_RowDeleting">

<Columns>

<asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />

<asp:BoundField DataField="FullName" HeaderText="Full Name" />

<asp:BoundField DataField="Email" HeaderText="Email" />

<asp:BoundField DataField="CreatedDate" HeaderText="Created Date" ReadOnly="True" />

<asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>