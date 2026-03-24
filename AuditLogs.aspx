<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLogs.aspx.cs" Inherits="MandarinQuest.AuditLogs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

<title>Audit Logs</title>

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
    overflow-x:auto;
}

/* TABLE STYLE */
table{
    width:100%;
    border-collapse:collapse;
}

th{
    background:#b91c1c;
    color:white;
    padding:10px;
    text-align:left;
}

td{
    padding:10px;
    border-bottom:1px solid #eee;
    word-wrap:break-word;
}

/* BUTTON */
.btn{
    padding:6px 12px;
    border:none;
    border-radius:6px;
    background:#dc2626;
    color:white;
    cursor:pointer;
    margin-bottom:15px;
}

.btn:hover{
    background:#b91c1c;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<h1>Audit Logs</h1>

<asp:Button ID="btnBack" runat="server"
Text="← Back to Admin"
CssClass="btn"
OnClick="btnBack_Click" />

<div class="card">

<asp:GridView
ID="gvAuditLogs"
runat="server"
AutoGenerateColumns="False"
AllowPaging="True"
PageSize="20"
OnPageIndexChanging="gvAuditLogs_PageIndexChanging"
OnRowDataBound="gvAuditLogs_RowDataBound">

<Columns>

    <asp:BoundField DataField="UserID" HeaderText="User ID" />

    <asp:BoundField DataField="Action" HeaderText="Action" />

    <asp:BoundField DataField="Description" HeaderText="Description" />

    <asp:BoundField DataField="LogDate"
        HeaderText="Date & Time"
        DataFormatString="{0:yyyy-MM-dd HH:mm}" />

</Columns>

</asp:GridView>

</div>

</div>

</form>

</body>
</html>