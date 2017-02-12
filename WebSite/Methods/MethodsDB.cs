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

        public List<string> FindTeachersDisciplines(int id)
        {
            List<string> result = new List<string>();

            var disciplines = db.tblDisciplines.ToList();
            var isChecked = db.tblDisciplinesMapping.Where(p => p.TeacherID == id && p.IsActive >= 0).Select(p => p.DisciplineID).ToArray();
            foreach (var item in isChecked)
            {
                result.Add(disciplines.Where(d => d.ID == item).Select(d => d.Name).FirstOrDefault());
            }
            return result;
        }
    }
}