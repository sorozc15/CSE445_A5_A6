# Farzad / Member C priority service

Canonical source: `aspx/C/TaskNotes.PartC/PriorityService.asmx` and `PriorityService.asmx.cs`.

This is an ASMX SOAP/WSDL service; Steven's separate scheduler uses WCF .svc. These can coexist in ASP.NET Framework; they do not need the same endpoint extension.

Operation: CalculatePriority(int importance, int daysUntilDue, int estimatedHours).
Returns PriorityResult with Score (int), Level (string), Explanation (string).

Live service: http://webstrarportal.fulton.asu.edu/sites/website49/Page0/PriorityService.asmx
Live WSDL: http://webstrarportal.fulton.asu.edu/sites/website49/Page0/PriorityService.asmx?WSDL

For the integrated app, copy the endpoint and class into the web project and preserve the namespace/Class directive. If moved under services/C, update the TryIt's ResolveUrl endpoint accordingly. Keep the working standalone deployment intact while integrating. See aspx/C/INTEGRATION.md for ranges, scoring and ordering rules.
