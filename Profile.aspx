<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="MandarinQuest.Profile" %>

<!DOCTYPE html>

<html>
<head runat="server">
<title>My Profile</title>

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
font-size:22px;
font-weight:600;
}

/* Container */

.container{
display:flex;
justify-content:center;
padding:50px;
}

/* Card */

.card{
background:white;
padding:35px;
width:420px;
border-radius:12px;
box-shadow:0 6px 16px rgba(0,0,0,0.1);
}

.card h2{
margin-top:0;
margin-bottom:25px;
}

/* Inputs */

.input-group{
margin-bottom:18px;
}

.input-group label{
font-weight:600;
display:block;
margin-bottom:6px;
}

.input-group input{
width:100%;
padding:10px;
border-radius:6px;
border:1px solid #ccc;
}

/* Buttons */

.btn{
padding:10px 18px;
border:none;
border-radius:6px;
cursor:pointer;
font-weight:600;
margin-top:10px;
}

.update{
background:#3498db;
color:white;
}

.password{
background:#27ae60;
color:white;
}

.back{
background:#ff4b4b;
color:white;
margin-top:20px;
width:100%;
}

.message{
margin-top:15px;
font-weight:600;
color:green;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="header">
MandarinQuest — My Profile
</div>

<div class="container">

<div class="card">

<h2>Profile Information</h2>

<div class="input-group">
<label>Name</label>
<asp:TextBox ID="txtProfileName" runat="server"></asp:TextBox>
</div>

<div class="input-group">
<label>Email</label>
<asp:TextBox ID="txtProfileEmail" runat="server" ReadOnly="true"></asp:TextBox>
</div>

<asp:Button 
ID="btnUpdateProfile" 
runat="server"
Text="Update Profile"
CssClass="btn update"
OnClick="btnUpdateProfile_Click" />

<hr style="margin:25px 0;">

<h2>Change Password</h2>

<div class="input-group">
<label>Current Password</label>
<asp:TextBox ID="txtCurrentPass" runat="server" TextMode="Password"></asp:TextBox>
</div>

<div class="input-group">
<label>New Password</label>
<asp:TextBox ID="txtNewPass" runat="server" TextMode="Password"></asp:TextBox>
</div>

<div class="input-group">
<label>Confirm New Password</label>
<asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password"></asp:TextBox>
</div>

<asp:Button
ID="btnChangePassword"
runat="server"
Text="Change Password"
CssClass="btn password"
OnClick="btnChangePassword_Click" />

<br />

<asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>

<asp:Button
ID="btnBackDashboard"
runat="server"
Text="Back to Dashboard"
CssClass="btn back"
OnClick="btnBackDashboard_Click"
/>

</div>

</div>

</form>

</body>
</html>