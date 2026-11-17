using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
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
        }

        private string[] ParseRoles(string roles)
        {
            string[] allowedRoles = { "Administrator", "Users" };
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
          if (string.IsNullOrEmpty(CurrentUsername))
            {
                Response.Redirect("Login.aspx");
                return;
            }
          if (!UserStore.IsInRole(CurrentUsername, "Administrator")) {
                Response.Redirect("Default.aspx");
                return;
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

    }
}