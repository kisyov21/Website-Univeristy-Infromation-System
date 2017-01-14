using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSite;
using WebSite.ViewModels.AdminViewModel;

namespace WebSite.Controllers
{
    public class AdminController : Controller
    {
        private WebSiteDBEntities db = new WebSiteDBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["CurrentUserPermissionLevel"] == null || (int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            else
            {
                var tblLogin = db.tblLogin.Include(t => t.tblTeachers);
                return View(tblLogin.ToList());
            }
        }

        public ActionResult AdminPage()
        {
            return View();
        }
        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLogin tblLogin = db.tblLogin.Find(id);
            if (tblLogin == null)
            {
                return HttpNotFound();
            }
            return View(tblLogin);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            AdminViewModel model = new AdminViewModel();
            //ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AdminViewModel login)
        {
            if (ModelState.IsValid)
            {
                tblLogin _login = new tblLogin() { LoginName = login.LoginName, Password = login.Password, PermissionLevel = (int)login.PermissionList, Course = (int)login.CourseList };
                try
                {
                    //Add newStudent entity into DbEntityEntry and mark EntityState to Added
                    db.Entry(_login).State = System.Data.Entity.EntityState.Added;

                    // call SaveChanges method to save new Student into database
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    foreach (var e in ex.EntityValidationErrors)
                    {
                        //check the ErrorMessage property
                    }
                }
            }
            return RedirectToAction("Index");
        }

     
        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLogin tblLogin = db.tblLogin.Find(id);
            if (tblLogin == null)
            {
                return HttpNotFound();
            }
            ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", tblLogin.TeacherID);
            return View(tblLogin);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LoginName,Password,PermissionLevel,Course,TeacherID")] tblLogin tblLogin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLogin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", tblLogin.TeacherID);
            return View(tblLogin);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLogin tblLogin = db.tblLogin.Find(id);
            if (tblLogin == null)
            {
                return HttpNotFound();
            }
            return View(tblLogin);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblLogin tblLogin = db.tblLogin.Find(id);
            db.tblLogin.Remove(tblLogin);
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
