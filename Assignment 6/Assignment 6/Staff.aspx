<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Staff.aspx.cs" Inherits="Assignment_6.Staff" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Exisiting Users</h1>
    <asp:Label ID="lblSessionStartTime" runat="server" />
    <asp:Label ID="lblCurrentUser" runat="server" />
    <asp:Label ID="lblnumofsessions" runat="server" />
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
    <div>
        <%if ((string)Session["Username"] == "TA")
            { %>
        <h1>Add New Staff</h1>
        <asp:Label ID="lblNewUsername" runat="server" Text="Username:" />
        <asp:TextBox ID="txtNewUsername" runat="server" />
        <asp:Label ID="lblNewPassword" runat="server" Text="Password:" />
        <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" />
        <asp:Button ID="btnAddStaff" runat="server" Text="Add Staff" OnClick="btnAddStaff_Click" />
        <asp:Label ID="lblAddUserMessage" runat="server" ForeColor="Red" />
        <%} %>
    </div>
</asp:Content>
