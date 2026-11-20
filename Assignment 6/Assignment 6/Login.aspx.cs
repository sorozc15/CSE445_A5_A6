using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace Assignment_6
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
     
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!CaptchaLogin.IsValidCode())
            {
                lblMessage.Text = "Invalid captcha code.";
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (UserStore.ValidateUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!CaptchaLogin.IsValidCode())
            {
                lblMessage.Text = "Invalid captcha code.";
                return;
            }

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (UserStore.RegisterUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);
                Session["Username"] = username;
                Response.Redirect("~/Default.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                lblMessage.Text = "Registration failed. Username may already exist.";
            }
        }
    }
}