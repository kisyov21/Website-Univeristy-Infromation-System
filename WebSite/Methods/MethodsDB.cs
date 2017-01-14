using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebSite.Methods
{
    public class MethodsDB
    {
        WebSiteDBEntities db = new WebSiteDBEntities();
        public bool CheckDBConnection()
        {
            try
            {
                db.Database.Connection.Open();
                db.Database.Connection.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}