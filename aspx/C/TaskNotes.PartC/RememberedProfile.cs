using System;
using System.Web;
namespace TaskNotes.PartC {
    // A preference cookie is NOT proof of authentication. Never put passwords in it.
    public static class RememberedProfile {
        private const string CookieName="TaskNotes.PartC.Username";
        public const string SessionKey="PartC.DemoUsername";
        public static string Read(HttpContext context) {
            HttpCookie cookie=context.Request.Cookies[CookieName];
            string name=cookie == null ? "" : cookie.Value;
            return IsValid(name) ? name : "";
        }
        private static bool IsValid(string name) {
            if (String.IsNullOrEmpty(name) || name.Length > 50) return false;
            foreach(char c in name) if (!Char.IsLetterOrDigit(c) && c!='_' && c!='-' && c!='.') return false;
            return true;
        }
        public static void Save(HttpContext context, string username, bool remember) {
            if (!IsValid(username)) throw new ArgumentException("Username must be 1-50 letters, digits, underscores, dots or hyphens.");
            context.Session[SessionKey]=username;
            WriteCookie(context, remember ? username : "", remember ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(-1));
        }
        public static void Clear(HttpContext context) {
            context.Session.Remove(SessionKey);
            WriteCookie(context, "", DateTime.UtcNow.AddDays(-1));
        }
        private static void WriteCookie(HttpContext context, string value, DateTime expiry) {
            // Application-scoped cookie avoids collisions with other WebStrar Page slots.
            string path=context.Request.ApplicationPath;
            if (String.IsNullOrEmpty(path)) path="/";
            HttpCookie cookie=new HttpCookie(CookieName, value) { HttpOnly=true, Secure=context.Request.IsSecureConnection, Path=path, Expires=expiry };
            context.Response.Cookies.Set(cookie);
        }
        // Call ONLY after the team's XML-backed access control validates credentials.
        public static void OnAuthenticatedLogin(HttpContext context, string username, bool rememberUsername) {
            Save(context, username, rememberUsername);
            System.Web.Security.FormsAuthentication.SetAuthCookie(username, false);
        }
        public static void SignOut(HttpContext context) {
            System.Web.Security.FormsAuthentication.SignOut();
            Clear(context);
            context.Session.Clear();
            context.Session.Abandon();
        }
    }
}
