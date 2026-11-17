<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="Assignment_6.Staff" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Staff Management</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Exisiting Users</h1>
        <asp:GridView ID="gvUsers" runat="server"
            AutoGenerateColumns="false"
            DataKeyNames="Username"
            AutoGenerateEditButton="false"
            OnRowEditing="gvUsers_RowEditing"
            OnRowCancelingEdit="gvUsers_RowCancelingEdit"
            OnRowUpdating="gvUsers_RowUpdating"
            OnRowDeleting="gvUsers_RowDeleting"
            EmptyDataText="No users found.">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="true" />
                <asp:TemplateField HeaderText="Roles">
                    <ItemTemplate>
                        <%# Eval("RolesText") %>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEditRoles" runat="server" Text='<%# Bind("RolesText") %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" EditText="Edit Roles" DeleteText="Delete User" />
            </Columns>
            </asp:GridView>
    </form>
</body>
</html>
