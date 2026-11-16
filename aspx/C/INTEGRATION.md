# Integrating Farzad's Part C

## Files to merge

- RememberedProfile.cs, ErrorLogger.cs, PriorityService.asmx and PriorityService.asmx.cs.
- Merge the Application_Error method from Global.asax.cs into the existing team's Global class. Keep the team's one Global.asax directive and its existing startup/session event methods. Add required using directives.
- Add System.Web.Services and System.Xml.Linq references and compile items to the team's Web Application project.
- Keep Farzad's namespace TaskNotes.PartC, or consistently change all namespaces plus the ASMX Class directive and references.
- Merge a default directory row and the three TryIt sections into the team's main page, or keep this page under a new name with its matching code-behind class. Update .aspx directives if renaming it. Do not overwrite the shared Default.aspx without coordinating.
- Preserve the team's Web.config; merge only needed configuration. Enable HttpPost under system.web/webServices/protocols for the supplied HTTP TryIt client. Keep Forms authentication; do not add anonymous allow rules over protected areas.
- Merge an empty App_Data/ErrorLog.xml only when no existing log needs preservation. Ensure IIS can write that directory.

## Login integration (after A/B verify XML credentials)

```csharp
// Only in the successful branch of the real login validation:
TaskNotes.PartC.RememberedProfile.OnAuthenticatedLogin(
    HttpContext.Current, verifiedUsername, rememberUsernameChecked);
```

This stores a demo/profile session entry, updates or expires the remembered-username cookie, and issues a NON-persistent Forms Authentication cookie. It does not validate credentials. Do not call it just because a preference cookie or submitted username exists. The session username is profile state, not an authorization check.

On the login page's first request, prefill the username textbox using `RememberedProfile.Read(HttpContext.Current)`; HTML-encode any value that is rendered as HTML. ASP.NET TextBox handles its own encoding.

On logout, call `RememberedProfile.SignOut(HttpContext.Current)` then redirect to the public page. It clears Forms Authentication, expires the preference cookie, clears session contents and abandons the session. Protected Member/Staff pages must still use the team's Forms authorization and server-verified role checks; a member login must never authorize Staff.

Cookies are HttpOnly, scoped to Request.ApplicationPath and Secure when the request uses HTTPS. HTTP WebStrar hosting cannot use Secure cookies; deploy HTTPS when available. Passwords and authentication tokens are never put in the username cookie. Because the starter targets Framework 4.0, it does not use the newer SameSite API; use the team's hosting/framework policy when available.

## Calculator contract

Endpoint: application-relative `PriorityService.asmx`. WSDL: `PriorityService.asmx?WSDL`.
SOAP namespace: `urn:tasknotes:partc`.
Operation: `CalculatePriority(int importance, int daysUntilDue, int estimatedHours)`.
Response: `PriorityResult { int Score; string Level; string Explanation; }`.

Input ranges: importance 1–5, daysUntilDue -365–3650, estimatedHours 1–100. Negative days mean overdue; 0 is today. Invalid direct service calls raise a service fault. The TryIt page validates inputs before making its HTTP POST.

Score = min(100, importance * 8 + urgency + effort).
Urgency: overdue 50; today 45; tomorrow 40; 2–3 days 30; 4–7 days 20; 8–14 days 10; 15+ days 0.
Effort: 8+ hours 10; 4–7 hours 5; 1–3 hours 0.
Levels: Critical >=75; High >=55; Medium >=30; otherwise Low.
This is a transparent project heuristic, not an industry standard.

To order tasks, call this service for each task and sort by Score descending, then due date ascending, then stable task ID. The client should calculate daysUntilDue from date-only values using the application's agreed timezone (for example Arizona), avoiding time-of-day rounding.

HTTP POST example from a same-origin client:
`PriorityService.asmx/CalculatePriority`
Content-Type: `application/x-www-form-urlencoded`
Body: `importance=5&daysUntilDue=0&estimatedHours=8`
Returns XML; expected Score=95 and Level=Critical. The supplied Default.aspx actually sends this request; it does not fake a service call by invoking PriorityRules directly.

## Error logging

Merge Application_Error, not a second Global.asax file. XML records contain no query string, exception message, request body, stack trace, password or cookie. The response shows a correlation ID only. XML logs are capped at 200 records and replaced via a temporary file. Locking assumes one IIS worker process; use a shared cross-process lock before enabling a web garden.

The generic HTML error response preserves HTTP status 500. ASMX faults are left to the service framework. To test the Global handler reliably, use the controlled ASPX error button. Remove that deliberate test endpoint or limit it to staff after grading.

## Ownership

Brian: A; Steven: B; Farzad: C. Only C source is supplied here. Agree the overall contribution percentages together before Assignment 6; do not infer them from this component list.
