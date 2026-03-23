<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminViewClasses.aspx.cs" Inherits="MandarinQuest.ViewClasses" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>View Classes</title>
</head>

<body>

<form id="form1" runat="server">

<h2>All Classes</h2>

Search Name
<asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>

Date
<asp:TextBox ID="txtDate" runat="server" TextMode="Date"></asp:TextBox>

Status
<asp:DropDownList ID="ddlStatus" runat="server">
    <asp:ListItem Text="All" Value="" />
    <asp:ListItem Text="Active" Value="Active" />
    <asp:ListItem Text="Cancelled" Value="Cancelled" />
</asp:DropDownList>

<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />

<br /><br />

<asp:GridView ID="gvClasses" runat="server" AutoGenerateColumns="true">
</asp:GridView>

</form>

</body>
</html>