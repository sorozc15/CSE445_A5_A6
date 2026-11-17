using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

// UserRecord class: Represents a user record with username and roles. Does not store password or other sensitive information.
// UserStore class: Static class. Used to manage user data from the xml file.

namespace Assignment_6
{
    public class UserRecord
    {
        public string Username { get; set; }
        public string RolesText { get; set; }
    }
    public static class UserStore
    {
        private static readonly object fileLock = new object();
        private static string FilePath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/Users.xml");
            }
        }
        
        private static XDocument LoadXml()
        {
            return XDocument.Load(FilePath);
        }

        private static void SaveXml(XDocument doc)
        {
            doc.Save(FilePath);
        }

        public static List<UserRecord> GetAllUsers()
        {
            lock(fileLock)
            {
                XDocument doc = LoadXml();
                return doc.Root.Elements("User")
                    .Select(user => new UserRecord
                    {
                        Username = (string)user.Element("Username"),
                        RolesText = string.Join(", ", user.Element("Roles").Elements("Role").Select(r => r.Value))
                    })
                    .ToList();
            }
        }

        public static bool IsInRole(string username, string role)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                XElement user = doc.Root.Elements("User")
                    .FirstOrDefault(u => string.Equals((string)u.Element("Username"),username,StringComparison.OrdinalIgnoreCase));
                return user != null && user.Element("Roles").Elements("Role")
                    .Any(r => string.Equals(r.Value, role, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static void AddUser(string username, string passwordHash, string[] roles)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                if (doc.Root.Elements("User").Any(u => string.Equals((string)u.Element("Username"), username, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception($"User '{username}' already exists.");
                }
                XElement newUser = new XElement("User",
                    new XElement("Username", username),
                    new XElement("PasswordHash", passwordHash),
                    new XElement("Roles", roles.Select(r => new XElement("Role", r)))
                );
                doc.Root.Add(newUser);
                SaveXml(doc);
            }
        }

        public static void UpdateRoles(string username, string[] roles)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                XElement user = doc.Root.Elements("User")
                    .FirstOrDefault(u => string.Equals((string)u.Element("Username"), username, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    throw new Exception($"User '{username}' not found.");
                }
                user.Element("Roles").RemoveAll();
                foreach (string role in roles)
                {
                    user.Element("Roles").Add(new XElement("Role", role));
                }
                SaveXml(doc);
            }
        }

        public static void DeleteUser(string username)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                XElement user = doc.Root.Elements("User")
                    .FirstOrDefault(u => string.Equals((string)u.Element("Username"), username, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    throw new Exception($"User '{username}' not found.");
                }
                user.Remove();
                SaveXml(doc);
            }
        }

        public static bool ValidateUser(string username, string password)
        {
            return true; // Placeholder for actual password validation logic
        }
    }
}