using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
                Session["Username"] = username;
                Response.Redirect("Default.aspx");
            }
            else
            {
                lblMessage.Text = "Invalid username or password.";
            }
        }
    }
}