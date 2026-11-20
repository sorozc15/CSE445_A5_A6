using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using SecurityLib;

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
        
        //Loads Users.xml
        private static XDocument LoadXml()
        {
            return XDocument.Load(FilePath);
        }

        //Saves to Users.xml
        private static void SaveXml(XDocument doc)
        {
            doc.Save(FilePath);
        }

        //Grabs all the users as a record
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

        //Checks to see if user has a specific role
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

        //Adds new user to the users.xml
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

        //Updates a user's roles given a list of roles
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

        // Deletes user
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

        //Checks to see if there is a user with a specific username and password
        public static bool ValidateUser(string username, string password)
        {
            lock (fileLock)
            {

                XDocument doc = LoadXml();
                XElement user = doc.Root.Elements("User")
                    .FirstOrDefault(u => string.Equals(
                        (string)u.Element("Username"),
                        username,
                        StringComparison.OrdinalIgnoreCase));

                if (user == null)
                    return false;

                string storedHash = (string)user.Element("PasswordHash");
                if (string.IsNullOrWhiteSpace(storedHash))
                    return false;

                bool result = SecurityLib.Crypto.Matches(password, storedHash);

                return result;
            }
        }

        //Registers a new user with a username and password
        public static bool RegisterUser(string username, string password)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                if (doc.Root.Elements("User").Any(u => string.Equals((string)u.Element("Username"), username, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
                string passwordHash = SecurityLib.Crypto.Encrypt(password);
                XElement newUser = new XElement("User",
                    new XElement("Username", username),
                    new XElement("PasswordHash", passwordHash),
                    new XElement("Roles", new XElement("Role", "User"))
                );
                doc.Root.Add(newUser);
                SaveXml(doc);
                return true;
            }
        }

        //Registers a new staff memember
        public static bool RegisterStaff(string username, string password)
        {
            lock (fileLock)
            {
                XDocument doc = LoadXml();
                if (doc.Root.Elements("User").Any(u => string.Equals((string)u.Element("Username"), username, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
                string passwordHash = SecurityLib.Crypto.Encrypt(password);
                XElement newUser = new XElement("User",
                    new XElement("Username", username),
                    new XElement("PasswordHash", passwordHash),
                    new XElement("Roles", new XElement("Role", "Staff"))
                );
                doc.Root.Add(newUser);
                SaveXml(doc);
                return true;
            }
        }
    }
}