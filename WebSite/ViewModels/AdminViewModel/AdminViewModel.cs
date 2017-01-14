using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebSite.Enum;

namespace WebSite.ViewModels.AdminViewModel
{
    public class AdminViewModel
    {     
        public string LoginName { get; set; }
        public string Password { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Select a correct permission")]
        public PermissionList PermissionList { get; set; }

        public CourseList CourseList { get; set; }
    }
}