using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
        private WebSiteDBEntities db = new WebSiteDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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