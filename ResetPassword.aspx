<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MandarinQuest.ResetPassword" %>

<form runat="server">

<h2>Reset Password</h2>

User ID

<asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>

New Password

<asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>

<asp:Button 
ID="btnReset"
runat="server"
Text="Reset"
OnClick="btnReset_Click"/>

</form>