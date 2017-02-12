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
        public List<string> Disciplines { get; set; }
        public TeachersViewModel()
        {

        }
        public TeachersViewModel(
            int id, string firstName, string lastName, string department, string courses,string education, string interests, string email, string visitingHours, string phoneNumber, string cabinet, List<string> disciplines)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Department = department;
            this.Courses = courses;
            this.Education = education;
            this.ScientificInterests = interests;
            this.Email = email;
            this.VisitingHours = visitingHours;
            this.PhoneNumber = phoneNumber;
            this.PersonalCabinet = cabinet;
            this.Disciplines = disciplines;
        }
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