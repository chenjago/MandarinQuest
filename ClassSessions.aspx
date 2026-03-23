<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassSessions.aspx.cs" Inherits="MandarinQuest.ClassSessions" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>MandarinQuest Class Sessions</title>

<style>

body{
margin:0;
font-family:'Segoe UI';
background:#f4f6f9;
}

.container{
width:90%;
margin:auto;
padding-top:40px;
}

.card{
background:white;
padding:25px;
border-radius:10px;
box-shadow:0 3px 10px rgba(0,0,0,0.1);
}

.grid{
width:100%;
border-collapse:collapse;
margin-top:20px;
}

.grid th{
background:#1e293b;
color:white;
padding:10px;
}

.grid td{
padding:10px;
border-bottom:1px solid #ddd;
}

.joinbtn{
background:#2563eb;
color:white;
border:none;
padding:8px 15px;
border-radius:6px;
cursor:pointer;
}

.joinbtn:hover{
background:#1d4ed8;
}

</style>

<script>

function copyLink(link) {

navigator.clipboard.writeText(link).then(function () {

alert("Session link copied!");

}, function (err) {

alert("Failed to copy link");

});

}

</script>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<div class="card">

<h2>Available Class Sessions</h2>

<asp:GridView
ID="gvSessions"
runat="server"
CssClass="grid"
AutoGenerateColumns="False">

<Columns>

<asp:BoundField DataField="SessionID" HeaderText="Session ID" />

<asp:BoundField DataField="SessionTitle" HeaderText="Title" />

<asp:BoundField DataField="SessionDate" HeaderText="Date" />

<asp:TemplateField HeaderText="Copy Link">

<ItemTemplate>

<asp:Button
ID="btnCopy"
runat="server"
Text="Copy Link"
CssClass="joinbtn"
OnClientClick='<%# "copyLink(\"" + Eval("SessionLink") + "\"); return false;" %>' />

</ItemTemplate>

</asp:TemplateField>

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>