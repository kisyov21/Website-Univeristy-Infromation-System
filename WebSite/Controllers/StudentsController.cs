using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSite;

namespace WebSite.Controllers
{
    public class StudentsController : Controller
    {
        private WebSiteDBEntities db = new WebSiteDBEntities();

        // GET: Students
        public ActionResult Index()
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            return View(db.tblStudents.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudents tblStudents = db.tblStudents.Find(id);
            if (tblStudents == null)
            {
                return HttpNotFound();
            }
            return View(tblStudents);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,Email,Interests,Employer,JobPosition,FacebookAcc,Linkedin")] tblStudents tblStudents)
        {
            if (ModelState.IsValid)
            {
                db.tblStudents.Add(tblStudents);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblStudents);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudents tblStudents = db.tblStudents.Find(id);
            if (tblStudents == null)
            {
                return HttpNotFound();
            }
            return View(tblStudents);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,Email,Interests,Employer,JobPosition,FacebookAcc,Linkedin")] tblStudents tblStudents)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblStudents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblStudents);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblStudents tblStudents = db.tblStudents.Find(id);
            if (tblStudents == null)
            {
                return HttpNotFound();
            }
            return View(tblStudents);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            tblStudents tblStudents = db.tblStudents.Find(id);
            db.tblStudents.Remove(tblStudents);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
