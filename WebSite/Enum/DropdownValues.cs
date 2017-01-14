using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSite.ViewModels.AdminViewModel;

namespace WebSite.Enum
{
    //public class DropdownValues
    //{
        // ------------ WILL BE REPLACED BY ENUM
        //public static IEnumerable<PermissionsList> PermList = new List<PermissionsList> 
        //{
        //    new PermissionsList
        //    {
        //        permissionCode = 1,
        //        permissionName = "Administrator"
        //    },
        //    new PermissionsList
        //    {
        //        permissionCode = 2,
        //        permissionName = "Teacher"
        //    },
        //    new PermissionsList
        //    {
        //        permissionCode = 3,
        //        permissionName = "Student"
        //    }
        //};
        //public static IEnumerable<CourseList> CourseList = new List<CourseList>
        //{
        //    new CourseList
        //    {
        //        courseID = 1,
        //        courseName = "First course"
        //    },
        //    new CourseList
        //    {
        //        courseID = 2,
        //        courseName = "Second course"
        //    },
        //    new CourseList
        //    {
        //        courseID = 3,
        //        courseName = "Third course"
        //    },
        //    new CourseList
        //    {
        //        courseID = 4,
        //        courseName = "Fourth course"
        //    },
        //    new CourseList
        //    {
        //        courseID = 5,
        //        courseName = "Master first course"
        //    },
        //    new CourseList
        //    {
        //        courseID = 6,
        //        courseName = "Master second course"
        //    },
        //};

        public enum PermissionList
        {
            Administrator = 1,
            Teacher = 2,
            Student = 3

        }
        public enum CourseList
        {
            First = 1,
            Second = 2,
            Third = 3,
            Fourth = 4,
            MFirst = 5,
            MSecond = 6
        }
   // }
}