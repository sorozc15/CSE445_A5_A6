using System;
using System.IO;
using System.Web;
using System.Xml.Linq;
using System.Linq;
namespace TaskNotes.PartC {
    public static class ErrorLogger {
        private static readonly object Gate=new object();
        public static string Log(HttpContext context, Exception error) {
            string id=Guid.NewGuid().ToString("N");
            string file=context.Server.MapPath("~/App_Data/ErrorLog.xml");
            lock(Gate) {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
                XDocument doc=File.Exists(file) ? XDocument.Load(file) : new XDocument(new XElement("errors"));
                // No query strings, passwords, cookies, request bodies, or stack traces are persisted.
                doc.Root.Add(new XElement("error",new XAttribute("id",id),
                    new XElement("utc", DateTime.UtcNow.ToString("o")),
                    new XElement("path",context.Request.AppRelativeCurrentExecutionFilePath),
                    new XElement("type",error.GetBaseException().GetType().FullName)));
                while(doc.Root.Elements("error").Count()>200) doc.Root.Elements("error").First().Remove();
                string temp=file+".tmp";
                doc.Save(temp);
                if(File.Exists(file)) File.Replace(temp,file,null); else File.Move(temp,file);
            }
            return id;
        }
    }
}
