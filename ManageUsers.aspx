<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUsers.aspx.cs" Inherits="MandarinQuest.ManageUsers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Users</title>

    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #f9fafb;
            padding: 40px 20px;
        }

        .container {
            max-width: 900px;
            margin: auto;
        }

        h1 {
            text-align: center;
            color: #b91c1c;
            margin-bottom: 25px;
        }

        .card {
            width: 100%;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.08);
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed;
        }

        th {
            background: #b91c1c;
            color: white;
            padding: 10px;
            text-align: left;
        }

        td {
            padding: 10px;
            border-bottom: 1px solid #eee;
            overflow: hidden;
            word-wrap: break-word;
        }

        input[type="text"],
        input[type="password"],
        select {
            width: 100%;
            max-width: 100%;
            box-sizing: border-box;
            padding: 6px;
            font-size: 14px;
        }

        .btn {
            padding: 6px 10px;
            border: none;
            border-radius: 6px;
            background: #dc2626;
            color: white;
            cursor: pointer;
            margin-bottom: 15px;
        }

        .btn:hover {
            background: #b91c1c;
        }

        .filter {
            margin-bottom: 15px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .filter label {
            font-weight: bold;
        }

        .filter select {
            width: 200px;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">
    <div class="container">

        <h1>Manage Users</h1>

        <asp:Button ID="btnBack" runat="server"
                    Text="← Back"
                    CssClass="btn"
                    OnClick="btnBack_Click" />

        <div class="card">

            <!-- Role Filter Dropdown -->
            <div class="filter">
                <label for="ddlFilterRole">Filter by Role:</label>
                <asp:DropDownList ID="ddlFilterRole" runat="server" AutoPostBack="true"
                                  OnSelectedIndexChanged="ddlFilterRole_SelectedIndexChanged">
                    <asp:ListItem Text="All" Value="All" />
                    <asp:ListItem Text="Admin" Value="Admin" />
                    <asp:ListItem Text="Teacher" Value="Teacher" />
                    <asp:ListItem Text="Student" Value="Student" />
                </asp:DropDownList>
            </div>

            <div style="margin-bottom:20px;">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlFilterRole_SelectedIndexChanged">
                </asp:DropDownList>

                <asp:TextBox ID="txtSearch" runat="server" placeholder="Search name or email"></asp:TextBox>

                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" />
            </div>

           <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="UserID"
                OnRowEditing="gvUsers_RowEditing"
                OnRowCancelingEdit="gvUsers_RowCancelingEdit"
                OnRowUpdating="gvUsers_RowUpdating"
                OnRowDeleting="gvUsers_RowDeleting">

                <Columns>
                    <asp:BoundField DataField="UserID" HeaderText="User ID" ReadOnly="True" />
                    <asp:TemplateField HeaderText="Full Name">
                        <ItemTemplate>
                            <%# Eval("FullName") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFullName" runat="server" Text='<%# Bind("FullName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <%# Eval("Email") %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="RoleName" HeaderText="Role" ReadOnly="True" />

                    <asp:TemplateField HeaderText="New Password">
                        <ItemTemplate>
                            ****** 
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>

        </div>
    </div>
</form>
</body>
</html>