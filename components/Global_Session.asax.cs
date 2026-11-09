using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Session_Global_Asax
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["ActiveSessions"] = 0;
        }

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

        protected void Session_End(object sender, EventArgs e)
        {
            Application.Lock();
            Application["ActiveSessions"] = (int)Application["ActiveSessions"] - 1;
            Application.UnLock();
        }
    }
}
