using System;
using System.Web;
namespace TaskNotes.PartC {
    public class Global : HttpApplication {
        protected void Application_Error(object sender, EventArgs e) {
            Exception error=Server.GetLastError();
            if(error==null) return;
            string id="unavailable";
            try { id=ErrorLogger.Log(Context,error); }
            catch(Exception logError) {
                // Do not recursively throw if IIS denies App_Data write access.
                System.Diagnostics.Trace.TraceError("Part C error logging failed: {0}",logError.GetType().Name);
            }
            // Let ASMX return its SOAP fault to service callers. The error is still logged.
            if(Request.Path.EndsWith(".asmx",StringComparison.OrdinalIgnoreCase)) return;
            Server.ClearError();
            Response.Clear();
            Response.StatusCode=500;
            Response.TrySkipIisCustomErrors=true;
            Response.ContentType="text/html";
            Response.Write("<!doctype html><html><head><title>TaskNotes error</title></head><body><h1>Request could not be completed</h1><p>Error reference: "+HttpUtility.HtmlEncode(id)+"</p><p>"+(id=="unavailable" ? "Logging failed. Check App_Data write permission." : "The error was recorded in App_Data/ErrorLog.xml.")+"</p><a href='"+HttpUtility.HtmlAttributeEncode(VirtualPathUtility.ToAbsolute("~/Default.aspx"))+"'>Return to TryIt</a></body></html>");
            CompleteRequest();
        }
    }
}
