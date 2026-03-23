<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LessonPreview.aspx.cs" Inherits="MandarinQuest.LessonPreview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body id="btnBackClasses">
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblLessonTitle" runat="server" Text="LessonPreview"></asp:Label>
            <br />
            <asp:TextBox ID="txtPreviewContent" runat="server"></asp:TextBox>
            <br />
            <br />
        </div>
    </form>
</body>
</html>
