using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.ViewModels.TeachersViewModel
{
    public class TeachersViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Courses { get; set; }
        public string Education { get; set; }
        public string ScientificInterests { get; set; }
        public string Email { get; set; }
        public string VisitingHours { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonalCabinet { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Disciplines { get; set; }
    }

    public class Disciplines
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool isSelected { get; set; }
        public Disciplines(int id, string name, bool isseleted)
        {
            this.ID = id;
            this.Name = name;
            this.isSelected = isseleted;
        }
    }
}