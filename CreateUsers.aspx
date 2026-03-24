<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUsers.aspx.cs" Inherits="MandarinQuest.CreateUsers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create User - MandarinQuest Admin</title>
    <style>
        body {
            font-family: 'Segoe UI';
            background: #f9fafb;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            margin: 0;
        }
        .container {
            width: 400px;
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 6px 15px rgba(0,0,0,0.1);
        }
        h1 {
            color: #b91c1c;
            text-align: center;
            margin-bottom: 20px;
        }
        label {
            display: block;
            margin-top: 15px;
            margin-bottom: 5px;
        }
        input[type=text], input[type=email], input[type=password], select {
            width: 100%;
            padding: 10px;
            border-radius: 8px;
            border: 1px solid #ccc;
        }
        input[type=submit], input[type=button] {
            margin-top: 20px;
            width: 100%;
            padding: 12px;
            border: none;
            border-radius: 10px;
            background: #dc2626;
            color: white;
            font-size: 16px;
            cursor: pointer;
            transition: 0.2s;
        }
        input[type=submit]:hover, input[type=button]:hover {
            background: #b91c1c;
        }
        .message {
            margin-top: 15px;
            text-align: center;
            color: green;
        }
        .error {
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Create User</h1>
            
            <label for="txtFullName">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server" />

            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" />

            <label for="txtPassword">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />

            <label for="ddlRoles">Role</label>
            <asp:DropDownList ID="ddlRoles" runat="server" />

            <asp:Button ID="btnCreateUser" runat="server" Text="Create User" OnClick="btnCreateUser_Click" />
            <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message" />
        </div>
    </form>
</body>
</html>