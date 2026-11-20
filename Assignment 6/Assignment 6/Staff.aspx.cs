using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Assignment_6
{
    public partial class Staff : System.Web.UI.Page
    {
        private string CurrentUsername
        {
            get
            {
                return Session["Username"] as string;
            }
        }

        private void LoadUsers()
        {
            gvUsers.DataSource = UserStore.GetAllUsers();
            gvUsers.DataBind();
            foreach (GridViewRow row in gvUsers.Rows)
            {
                string username = row.Cells[0].Text;
                if (UserStore.IsInRole(CurrentUsername, "Staff"))
                {
                    if (!UserStore.IsInRole(username, "User"))
                    {
                        gvUsers.Rows[row.RowIndex].Cells[2].Enabled = false;
                    }
                }
                if (string.Equals(username, CurrentUsername, StringComparison.OrdinalIgnoreCase))
                {
                    gvUsers.Rows[row.RowIndex].Cells[2].Enabled = false;
                }
            }
        }

        private string[] ParseRoles(string roles)
        {
            string[] allowedRoles = { "Administrator", "Staff", "Users" };
            string[] result = roles.Split(',')
                .Select(r => r.Trim())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            if (result.Length == 0)
            {
                throw new Exception("At least one role must be specified.");
            }
            foreach (string role in result)
            {
                if (!allowedRoles.Contains(role, StringComparer.OrdinalIgnoreCase))
                {
                    throw new Exception($"Invalid role: {role}. Allowed roles are: {string.Join(", ", allowedRoles)}");
                }
            }
            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                Response.Redirect("~/Login.aspx");
                Context.ApplicationInstance.CompleteRequest();
                return;
            }
            if (!UserStore.IsInRole(CurrentUsername, "Administrator") && !UserStore.IsInRole(CurrentUsername, "Staff"))
            {
                Response.Redirect("~/Default.aspx");
                return;
            }
            if (!IsPostBack)
            {
                LoadUsers();
                lblSessionStartTime.Text = $"Session started at: {Session["SessionStarted"]}";
                lblCurrentUser.Text = $"Current user: {CurrentUsername}";
                lblnumofsessions.Text = $"Active sessions: {Application["ActiveSessions"]}";
            }

        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string username = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtEditRoles = (TextBox)gvUsers.Rows[e.RowIndex].FindControl("txtEditRoles");
            try
            {
                UserStore.UpdateRoles(username, ParseRoles(txtEditRoles.Text));
                gvUsers.EditIndex = -1;
                LoadUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating roles for user {username}: {ex.Message}");
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string username = gvUsers.DataKeys[e.RowIndex].Value.ToString();
            if (string.Equals(username, CurrentUsername, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Cannot delete the currently logged-in user.");
                return;
            }
            try
            {
                UserStore.DeleteUser(username);
                LoadUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user {username}: {ex.Message}");
            }
        }

        protected void btnAddStaff_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text;

            if (UserStore.RegisterStaff(username, password))
            {
                LoadUsers();
                txtNewUsername.Text = string.Empty;
                txtNewPassword.Text = string.Empty;
            }
            else
            {
                Console.WriteLine($"Error adding new staff user, username may already exist.");
            }
        }
    }
}