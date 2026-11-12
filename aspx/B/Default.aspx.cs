using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment_5
{
    public partial class _Default : Page
    {
  
        protected void Page_Load(object sender, EventArgs e)
        {
            //Initialize the label for the session handler global.asax
            lblSession.Text =
                "Session ID: " + Session.SessionID + "<br />" +
                "Username: " + Session["Username"] + "<br />" +
                "IsLoggedIn: " + Session["IsLoggedIn"] + "<br />" +
                "SessionStarted: " + Session["SessionStarted"] + "<br />" +
                "Active sessions: " + Application["ActiveSessions"];

            //Initialize the date time selector for the task scheduler
            if (!IsPostBack)
            {
                txtDateTime.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
            }
        }

        //Submit function for the captcha
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblMessage.Text = Captcha.IsValidCode() ? "Captcha correct" : "Captcha incorrect";
        }

        //Sign In button for the session handler
        protected void btnSetUser_Click(object sender, EventArgs e)
        {
            Session["Username"] = txtUser.Text;
            Session["IsLoggedIn"] = true;
            Response.Redirect(Request.RawUrl);
        }

        //Log Out button for the session handler
        protected void btnAbandon_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect(Request.RawUrl);
        }

        //Task Scheduler Button given the date and frequency returns the next 3 dates
        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            TaskScheduler.ServiceClient client = new TaskScheduler.ServiceClient();

            try
            {
                DateTime[] result = client.GetOccurrences(DateTime.Parse(txtDateTime.Text), rblOptions.SelectedValue, 3);
                lblScheduledDates.Text = "Next three scheduled dates:";
                for (int i = 0; i < result.Length; i++)
                {
                    lblScheduledDates.Text += "<br />" + result[i].ToString();
                }
                client.Close();
            }
            catch (Exception ex)
            {
                lblScheduledDates.Text = ex.Message;
            }
        }

        //Gets the next occurence of a scheduled task given a date and frequency
        //DATE MUST BE BEFORE TODAY
        protected void btnNextDate_Click(object sender, EventArgs e)
        {
            DateTime userDate = DateTime.Parse(txtDateTime.Text);
            if (userDate >= DateTime.Today)
            {
                lblScheduledDates.Text = "!Date must be before today!";
                return;
            }
            TaskScheduler.ServiceClient client = new TaskScheduler.ServiceClient();

            try
            {
                DateTime result = client.GetNextOccurrence(userDate, rblOptions.SelectedValue);
                lblScheduledDates.Text = result.ToString();
                client.Close();
            }
            catch (Exception ex)
            {
                lblScheduledDates.Text = ex.Message;
            }
        }
    }
}