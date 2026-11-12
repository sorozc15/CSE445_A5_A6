<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Assignment_5._Default" %>
<%@ Register Src="~/Components/Captcha.ascx" TagPrefix="uc" TagName="Captcha" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Assignment 5 - Steven Orozco</h1>
        </section>
    <div>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Application and Components Summary Table</th>
                </tr>
                <tr>
                    <th>Provider Name</th>
                    <th>Page and Component Type</th>
                    <th>Component Description</th>
                    <th>Resources and Methods</th>
                    <th>Location</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Steven Orozco</td>
                    <td>User Control</td>
                    <td>Captcha generator that can be used to verify real human users.</td>
                    <td>C# Code as a ASP User Control file. Linked to the Default page.</td>
                    <td>~/Components/Captcha.ascx</td>
                </tr>
                <tr>
                    <td>Steven Orozco</td>
                    <td>Global.asax</td>
                    <td>Session handler: Initizalizes session and keeps tracked of users that are actively logged on.</td>
                    <td>C# Code as script in Global.asax file.</td>
                    <td>~/Global.asax</td>
                </tr>
                <tr>
                    <td>Steven Orozco</td>
                    <td>SVC Service</td>
                    <td>Task Scheduler: Given a day and time frequency (e.g. daily, weekly, etc.) returns a list of datetimes. Can also calculate the next occurance of a task given a date prior to the current day.</td>
                    <td>Interface and impmentation C# code developed behind a SVC service.</td>
                    <td>In the solution but used in Default.aspx as service reference from <a href="http://webstrarportal-env.eba-uzcvm8rb.us-west-2.elasticbeanstalk.com/sites/website49/Page10/Service.svc"> Deployment server</a></td>
                </tr>
            </tbody>
        </table>
    </div>
    <div>
        <h1>Captcha - User Control</h1>
        <uc:Captcha ID="Captcha" runat="server" />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
        <asp:Label ID="lblMessage" runat="server" />
    </div>
    <div>
        <h1>Session Handler - Global.asax</h1>
        <asp:Label ID="lblSession" runat="server" /><br /><br />
        Username
        <asp:TextBox ID="txtUser" runat="server" />
        <asp:Button ID="btnSetUser" runat="server" Text="Save to session" OnClick="btnSetUser_Click" />
        <asp:Button ID="btnAbandon" runat="server" Text="End session" OnClick="btnAbandon_Click" />
    </div>
    <div>
        <h1>Task Scheduler - SVC</h1>
        Schedule Task: <br />
        <asp:TextBox ID="txtDateTime" runat="server" TextMode="DateTimeLocal" CssClass="form-control" />
        <asp:RadioButtonList ID="rblOptions" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
            <asp:ListItem Text="Daily" Value="daily" Selected="True" />
            <asp:ListItem Text="Weekly" Value="weekly" />
            <asp:ListItem Text="Monthly" Value="monthly" />
        </asp:RadioButtonList>
        <asp:Button ID="btnSchedule" runat="server" Text="Schedule Task" OnClick="btnSchedule_Click" />
        <asp:Button ID="btnNextDate" runat="server" Text="Get Next Occurence" OnClick="btnNextDate_Click" /> <br />
        <asp:Label ID="lblScheduledDates" runat="server" />

    </div>
    </main>

</asp:Content>
