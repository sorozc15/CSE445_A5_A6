# TaskNotes — Member C / Farzad

Assignment 5 individual source submission. Team assignments: Brian (A), Steven (B), Farzad (C). This ZIP contains only Farzad's Part C. Do not claim teammates' components or an invented contribution percentage.

## Included

- ASP.NET Framework 4.0 Web Application solution (not ASP.NET Core).
- Cookie/session component: remembered username for 30 days, demo session for 20 minutes, explicit clear, verified-login integration hook, and sign-out hook.
- Global.asax Application_Error handler: private XML error log, limited to 200 records; UTC time, path, exception type and correlation ID; controlled test button.
- ASMX SOAP service: Task Priority Calculator, also callable by HTTP POST.
- Default.aspx: component directory, provider name, current URL, inputs/outputs, live HTTP service test, cookie/session tests and unhandled-error test.
- Priority rules test runner and PowerShell build/test/publish script.

## Current status

The user compiled the original solution successfully in Visual Studio/Apporto and deployed it to WebStrar Site 49/Page0. Screenshots confirmed the 95 Critical calculator result locally and live, saved username session/cookie locally, and the deliberate local XML error log. This copy incorporates the ViewState session-key fix and UTF-8 configuration used during those checks. It has not been rebuilt independently here; the full automated C# test suite has not been run. See DEPLOYMENT.txt for links and verification limits. Canvas submission has not been confirmed.

## Open and run on Windows / ASU Apporto

1. Use Visual Studio with **ASP.NET and web development** plus .NET Framework 4 targeting support. Open `PartC.sln`.
2. If Visual Studio cannot target 4.0, use a lab environment with that targeting pack. Do not silently retarget without checking the WebStrar compatibility instructions. The supplied Canvas note recommends 4.0.
3. Build the solution. Right-click TaskNotes.PartC and set it as startup project. Set Default.aspx as start page; run using IIS Express (Ctrl+F5).
4. Default.aspx is public. Test each of its three sections using TESTING.md.
5. Run `powershell -ExecutionPolicy Bypass -File .\Build-Test-Package.ps1` from this folder in a Visual Studio Developer PowerShell. It compiles the web DLL, executes priority tests, and builds `dist/FilesInPage0.zip`.

No third-party NuGet packages, database, API key or external service is required. The app calls its own deployed ASMX endpoint by HTTP. If that request is blocked by your host's proxy/network configuration, the GUI displays a service failure; investigate connectivity rather than replacing it with a local method call.

## Required deployment step

Assignment 5 requires your individually developed service on WebStrar; the instructions state a 2-point deduction if it only uses localhost. The full application deployment is optional for Assignment 5. Deploying this complete small package makes its TryIt interface available too.

1. Obtain your assigned site number and coordinate with Steven before replacing an existing Page slot. No site number or ASURITE is included here.
2. Build and test first. `dist/FilesInPage0.zip` contains Default.aspx, Web.config, ASMX, Global.asax, Styles.css, bin/TaskNotes.PartC.dll and App_Data/ErrorLog.xml at its ROOT. It is generated only after a successful build/test.
3. Alternatively use Visual Studio Publish > Folder, then ZIP the published folder's CONTENTS, not its parent folder.
4. Sign into the course's WebStrar portal with ASURITE. Upload to the agreed slot. Page0 must contain Default.aspx. An unused Page1–Page10 slot can also host this standalone app if assigned/approved by the team.
5. Open the live Default.aspx; run ALL tests. Open PriorityService.asmx?WSDL. Record the actual public URLs in DEPLOYMENT.txt before creating the final Canvas ZIP.
6. Ensure the application identity can write App_Data. The error test reports logging failure if it cannot; never enable public access to App_Data to fix write permission.

## What goes to Canvas

Upload ONE ZIP containing this complete solution, source, test instructions and actual deployed service/TryIt URLs. The current source ZIP is the starting package; after Windows testing/deployment update DEPLOYMENT.txt and zip the solution folder again. The published WebStrar ZIP alone does not contain the source required for code reading.

## Team integration

See INTEGRATION.md before copying into the shared app. ASP.NET applications have one Global.asax: merge Farzad's Application_Error into the team's existing class; do NOT replace Brian's startup or Steven's session handlers.

The Member/Staff buttons explain the Assignment 6 integration boundary. They are not implemented account pages. This Assignment 5 individual component demonstration intentionally does not create accounts or authenticate arbitrary demo usernames.
