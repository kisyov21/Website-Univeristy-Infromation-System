﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class HomeController : Controller
    {
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
            ViewBag.Message = "Information for the student.";

            return View();
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