//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebSite
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblLogin
    {
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public Nullable<int> PermissionLevel { get; set; }
        public Nullable<int> Course { get; set; }
        public Nullable<int> TeacherID { get; set; }
    
        public virtual tblTeachers tblTeachers { get; set; }
    }
}
