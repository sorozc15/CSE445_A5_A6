# Farzad / Member C component integration

Canonical source is in `aspx/C/TaskNotes.PartC/`:
- `RememberedProfile.cs`: username preference cookie, profile session, verified-login and logout hooks.
- `ErrorLogger.cs`: private XML error logging.
- `Global.asax.cs`: Application_Error event to merge.

Copy the two helper classes into the integrated Web Application project and include them in compilation. Merge only Application_Error from C into Assignment_5.Global in the existing components/Global.asax.cs. Keep Steven's Session_Start/Session_End and Brian's eventual Application_Start work. Keep one Global.asax per deployed application. C's standalone Global.asax must not overwrite the shared file.

Call RememberedProfile.OnAuthenticatedLogin only after real XML credential verification. Steven's btnSetUser_Click is a session demonstration, not credential authentication; never treat its IsLoggedIn boolean as authorization. Part C's demo profile deliberately remains unauthenticated.

Read aspx/C/INTEGRATION.md for API details. The repository snapshot lacks RouteConfig, BundleConfig, Web.config, project/solution files and the generated TaskScheduler.ServiceClient proxy referenced by B; resolve these from Steven's full project during integration. No teammate files have been changed by this addition.
