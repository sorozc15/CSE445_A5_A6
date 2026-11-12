using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Assignment_5
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["ActiveSessions"] = 0;
        }

        //Following functions are code for the session handler
        //Logs a new session that was started
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Username"] = "";
            Session["IsLoggedIn"] = false;
            Session["SessionStarted"] = DateTime.Now;
            Session["TaskCount"] = 0;

            Application.Lock();
            Application["ActiveSessions"] = (int)Application["ActiveSessions"] + 1;
            Application.UnLock();
        }

        //Logsout sessions that ended
        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveSessions"] = (int)Application["ActiveSessions"] - 1;
            Application.UnLock();
        }
    }
}