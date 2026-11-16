# Verification checklist (run on Windows/IIS Express, then WebStrar)

## Build and rules

Run Build-Test-Package.ps1. It must compile the web project and print that priority tests passed. The C# tests cover six explicit examples, six invalid-input cases and 21,000 valid combinations with bounds and monotonicity checks. A source-only check is not a substitute for this run.

## Service and integrated TryIt

1. Open PriorityService.asmx?WSDL: expect a WSDL document containing CalculatePriority.
2. On Default.aspx calculate 5,0,8: expect 95 / 100 Critical.
3. Calculate 3,7,2: expect 44 Medium. Calculate 1,30,1: expect 8 Low.
4. Calculate 5,-1,100: expect 100 Critical.
5. Use importance 0 or text: expect validation feedback, not a service score.
6. Directly POST invalid values to ASMX: expect a fault/error response. POST valid form values: expect XML PriorityResult.
7. Confirm the TryIt uses the deployed endpoint, not a localhost URL. In a local copy only, rename PriorityService.asmx and retry: the GUI should report a service failure. Restore the file.

## Cookie / Session

1. Fresh private window: session/cookie empty, Authenticated=False.
2. Save `Farzad` with Remember checked. Refresh: both session and cookie show Farzad, and username prefills. Authenticated must remain False.
3. Open another tab in the same browser: same session/profile.
4. Close ALL windows for that browser profile and reopen: username cookie remains up to 30 days; server session may expire after 20 idle minutes. Some browsers restore session cookies; do not use tab close as proof of session expiry.
5. Save another name with Remember unchecked. Refresh: session has new name, remembered cookie is absent.
6. Clear profile and refresh: demo session and remembered cookie absent.
7. Inspect cookie: HttpOnly, application path, expiration; Secure on HTTPS.
8. Test a username with `<` or a slash: invalid input must never be stored. ASP.NET request validation may reject `<` before the handler, which invokes the global error handler.
9. Member/Staff buttons explain integration; they do not grant access.

## Error handling

1. Click controlled test error: expect HTTP 500, generic error text and a reference ID.
2. Inspect App_Data/ErrorLog.xml from the filesystem: new record with that ID, UTC timestamp, ASPX path and exception type. Repeat: XML must remain well-formed with an additional record.
3. Request App_Data/ErrorLog.xml publicly: expect blocked access.
4. Follow Return to TryIt: expect the public page to load.
5. In a disposable local copy, deny App_Data writes and repeat: generic error should state logging failed, without recursive errors. Restore permissions.

## After merging with real authentication

1. Anonymous Member/Staff requests redirect to Login.
2. Valid member login sets Forms Authentication; invalid credentials never do.
3. A remembered username or forged preference cookie cannot access protected pages.
4. Member identity cannot access Staff. Staff credentials/roles are checked by the team's XML authentication component.
5. Logout expires authentication and clears profile/session. Protected pages then require login again.

## Creation-environment checks

XML configuration and project file parse; all compile/content includes exist; page handlers and control IDs match; ASMX and Global directives match implementation classes. These static checks do not establish successful compilation, ASP.NET execution or WebStrar availability.
