<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Captcha.ascx.cs" Inherits="Captcha" %>

<img id="imgCaptcha" runat="server" alt="Captcha image" /><br />
<asp:LinkButton ID="lnkRefresh" runat="server" CausesValidation="false">Get a new image</asp:LinkButton><br />
<asp:TextBox ID="txtCaptcha" runat="server" MaxLength="5" />