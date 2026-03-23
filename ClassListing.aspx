<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClassListing.aspx.cs" Inherits="MandarinQuest.ClassListing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:GridView ID="dgvClasses" runat="server">
        </asp:GridView>
        <br />
        <asp:Button ID="btnViewPreview" runat="server" Text="ViewPreview" />
        <br />
        <asp:Button ID="btnBackHome" runat="server" OnClick="btnBackHome_Click" Text="Back" />
    </form>
</body>
</html>
