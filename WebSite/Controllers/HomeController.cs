using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Methods;
using WebSite.ViewModels.TeachersViewModel;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private MethodsDB mdb = new MethodsDB();
        private WebSiteDBEntities db = new WebSiteDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Students()
        {
            ViewBag.Message = "Information for graduated students.";

            return View(db.tblStudents.ToList());
        }

        public ActionResult Teachers()
        {
            ViewBag.Message = "Information for teachers.";
            List<TeachersViewModel> model = new List<TeachersViewModel>();
            var allTeachers = db.tblTeachers.ToList();
            foreach (var item in allTeachers)
            {
                model.Add(new TeachersViewModel(item.ID,item.FirstName,item.LastName,item.Department,item.Courses,item.Education,item.ScientificInterests,item.Email,item.VisitingHours,item.PhoneNumber,item.PersonalCabinet, mdb.FindTeachersDisciplines(item.ID)));
            }
            return View(model);
        }


        public ActionResult Calendar()
        {
            ViewBag.Message = "Schedule";
            return View();
        }
        public ActionResult NoPermission()
        {
            return View("~/Views/Shared/NoPermission.cshtml");
        }
    }
}