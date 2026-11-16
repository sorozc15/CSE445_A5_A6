<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TaskNotes.PartC.TryIt" %>
<!doctype html><html lang="en"><head runat="server"><meta charset="utf-8"/><meta name="viewport" content="width=device-width, initial-scale=1"/><title>TaskNotes | Farzad, Part C</title><link href="Styles.css" rel="stylesheet"/></head>
<body><form id="MainForm" runat="server"><main>
<header><span class="tag">CSE 445 · ASSIGNMENT 5 · MEMBER C</span><h1>TaskNotes</h1><p>Prioritize your work. Keep your username handy. Test every component.</p></header>
<section><h2>Application &amp; component directory</h2><p>Provider: <strong>Farzad</strong>. This package contains Member C's individual contribution only; overall team contribution percentages are to be agreed by the team.</p><p>Current deployment: <asp:HyperLink ID="CurrentUrl" runat="server"/> · <a href="PriorityService.asmx?WSDL">Service WSDL</a></p>
<div class="table"><table><thead><tr><th>Type / operation</th><th>Input → output</th><th>Purpose / TryIt</th></tr></thead><tbody>
<tr><td>Cookie + Session<br/>RememberedProfile.Save / Read / Clear</td><td>username: string, remember: bool → session/cookie state</td><td>30-day username preference and 20-minute session. <a href="#profile">Test profile</a></td></tr>
<tr><td>Global.asax<br/>Application_Error / ErrorLogger.Log</td><td>unhandled Exception → XML error reference: string</td><td>Log UTC time, path and exception type. <a href="#errors">Test error handler</a></td></tr>
<tr><td>SOAP / ASMX<br/>CalculatePriority</td><td>importance: int (1–5), daysUntilDue: int (-365–3650), estimatedHours: int (1–100) → PriorityResult {Score:int, Level:string, Explanation:string}</td><td>Rank tasks by importance, deadline and effort. <a href="#priority">Test service</a></td></tr>
</tbody></table></div></section>
<section id="priority"><h2>01 / Task Priority Calculator</h2><p>This form makes a real HTTP request to PriorityService.asmx. Negative days mean overdue.</p><div class="grid">
<label>Importance (1–5)<asp:TextBox ID="Importance" Text="5" runat="server"/></label>
<label>Days until due<asp:TextBox ID="Days" Text="0" runat="server"/></label>
<label>Estimated hours<asp:TextBox ID="Hours" Text="8" runat="server"/></label></div>
<asp:Button runat="server" Text="Calculate priority" OnClick="Calculate_Click"/>
<asp:Label ID="PriorityOutput" CssClass="output" runat="server"/>
<p>Expected tests: (5, 0, 8) → 95 Critical; (3, 7, 2) → 44 Medium; (1, 30, 1) → 8 Low. Importance 0 is rejected.</p></section>
<section id="profile"><h2>02 / Cookie &amp; session</h2><p>Save a demo profile, refresh, then open this page in another tab. Close the browser and reopen to test the remembered username. A remembered username never grants account access. Registration and credential verification belong to the team's integrated application.</p>
<label>Username<asp:TextBox ID="Username" MaxLength="50" runat="server"/></label><p><asp:CheckBox ID="Remember" Text=" Remember username for 30 days" runat="server"/></p>
<asp:Button runat="server" Text="Save demo profile" OnClick="Save_Click"/>
<asp:Button runat="server" Text="Clear profile" CssClass="secondary" OnClick="Clear_Click"/>
<asp:Label ID="ProfileOutput" CssClass="output" runat="server"/>
<p><asp:Label ID="StateOutput" runat="server"/></p>
<p><asp:Button runat="server" Text="Member access" CssClass="secondary" OnClick="Member_Click"/> <asp:Button runat="server" Text="Staff access" CssClass="secondary" OnClick="Staff_Click"/></p>
<asp:Label ID="AccessOutput" CssClass="output" runat="server"/></section>
<section id="errors"><h2>03 / Global error handler</h2><p>This deliberate test raises an unhandled exception. Expect an HTTP 500 page with a reference ID; match it to App_Data/ErrorLog.xml in Visual Studio or the server filesystem. Logs are not publicly downloadable.</p>
<asp:Button runat="server" Text="Trigger controlled test error" OnClick="Error_Click" CssClass="secondary"/></section>
<footer>Part C • Farzad • ASP.NET Framework 4.0 • No passwords stored in cookies</footer>
</main></form></body></html>
