using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Linq;
namespace TaskNotes.PartC {
    public partial class TryIt : Page {
        protected HyperLink CurrentUrl;
        protected TextBox Importance, Days, Hours, Username;
        protected CheckBox Remember;
        protected Label PriorityOutput, ProfileOutput, StateOutput, AccessOutput;
        protected override void OnInit(EventArgs e) {
            base.OnInit(e);
            // Persist a random per-session key before rendering the first form.
            if (Session["PartC.ViewStateKey"] == null)
                Session["PartC.ViewStateKey"] = Guid.NewGuid().ToString("N");
            ViewStateUserKey = (string)Session["PartC.ViewStateKey"];
        }
        protected void Page_Load(object sender, EventArgs e) {
            CurrentUrl.Text=Request.Url.GetLeftPart(UriPartial.Path);
            CurrentUrl.NavigateUrl=Request.Url.GetLeftPart(UriPartial.Path);
            if(!IsPostBack) { Username.Text=RememberedProfile.Read(Context); Remember.Checked=Username.Text.Length>0; }
        }
        protected override void OnPreRender(EventArgs e) {
            StateOutput.Text=HttpUtility.HtmlEncode("Session demo username: "+(Session[RememberedProfile.SessionKey] as string ?? "(none)")+" | Incoming remembered cookie: "+(RememberedProfile.Read(Context)=="" ? "(none)" : RememberedProfile.Read(Context))+" | Authenticated: "+Request.IsAuthenticated+". Cookie updates appear after the next request.");
            base.OnPreRender(e);
        }
        protected void Save_Click(object sender, EventArgs e) {
            try { RememberedProfile.Save(Context,Username.Text.Trim(),Remember.Checked); ProfileOutput.Text="Demo profile saved. Refresh to inspect the new cookie. No authentication was performed."; }
            catch(ArgumentException ex) { ProfileOutput.Text=HttpUtility.HtmlEncode(ex.Message); }
        }
        protected void Clear_Click(object sender, EventArgs e) {
            RememberedProfile.Clear(Context); Username.Text=""; Remember.Checked=false;
            ProfileOutput.Text="Profile cleared; the preference cookie has been expired. Refresh to verify.";
        }
        protected void Member_Click(object sender, EventArgs e) { ExplainAccess("Member"); }
        protected void Staff_Click(object sender, EventArgs e) { ExplainAccess("Staff"); }
        private void ExplainAccess(string area) { AccessOutput.Text=area+" pages and XML account authorization are integrated by the team for Assignment 6. This Part C demo cannot authenticate users."; }
        protected void Error_Click(object sender, EventArgs e) { throw new InvalidOperationException("Part C controlled error-handler test."); }
        protected void Calculate_Click(object sender, EventArgs e) {
            int importance, days, hours;
            if(!Int32.TryParse(Importance.Text,out importance) || !Int32.TryParse(Days.Text,out days) || !Int32.TryParse(Hours.Text,out hours)) { PriorityOutput.Text="Enter whole numbers in every field."; return; }
            if(importance<1 || importance>5 || days< -365 || days>3650 || hours<1 || hours>100) { PriorityOutput.Text="Use importance 1–5, days -365–3650, and hours 1–100."; return; }
            try {
                // Build a same-application endpoint, so deployment needs no site-number edits.
                Uri endpoint=new Uri(Request.Url,ResolveUrl("~/PriorityService.asmx/CalculatePriority"));
                string body="importance="+importance+"&daysUntilDue="+days+"&estimatedHours="+hours;
                byte[] bytes=Encoding.UTF8.GetBytes(body);
                HttpWebRequest request=(HttpWebRequest)WebRequest.Create(endpoint);
                request.Method="POST"; request.ContentType="application/x-www-form-urlencoded"; request.Timeout=10000;
                request.ContentLength=bytes.Length;
                using(var stream=request.GetRequestStream()) stream.Write(bytes,0,bytes.Length);
                using(var response=request.GetResponse())
                using(var stream=response.GetResponseStream()) {
                    XDocument doc=XDocument.Load(stream);
                    Func<string,string> value=name=>doc.Root.Elements().First(x=>x.Name.LocalName==name).Value;
                    PriorityOutput.Text=HttpUtility.HtmlEncode(value("Score")+" / 100 · "+value("Level")+" — "+value("Explanation"));
                }
            } catch(WebException) { PriorityOutput.Text="Service call failed. Check PriorityService.asmx in this application, HTTP POST availability, and server connectivity."; }
        }
    }
}
