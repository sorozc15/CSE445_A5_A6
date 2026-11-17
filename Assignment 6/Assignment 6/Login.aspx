<!DOCTYPE html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="~/Login.aspx.cs" Inherits="Assignment_6.Login" %>
<%@ Register Src="~/Controls/Captcha.ascx" TagPrefix="uc" TagName="Captcha" %>
<html>
<head>
    <meta charset="utf-8" />
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Login</h1>
        <div>
            <asp:Label ID="lblUsername" runat="server" Text="Username:" />
            <asp:TextBox ID="txtUsername" runat="server" />
        </div>
        <br />
        <div>
            <asp:Label ID="lblPassword" runat="server" Text="Password:" />
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
        </div>
        <br />
        <div>
            <uc:Captcha ID="CaptchaLogin" runat="server" />
        </div>
        <br />
        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
        <br />
        <br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
    </form>
</body>
</html>