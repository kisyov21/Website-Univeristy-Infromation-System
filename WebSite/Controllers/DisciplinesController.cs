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
    public class DisciplinesController : Controller
    {
        private WebSiteDBEntities db = new WebSiteDBEntities();

        // GET: Disciplines
        public ActionResult Index()
        {
            return View(db.tblDisciplines.ToList());
        }

        // GET: Disciplines/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDisciplines tblDisciplines = db.tblDisciplines.Find(id);
            if (tblDisciplines == null)
            {
                return HttpNotFound();
            }
            return View(tblDisciplines);
        }

        // GET: Disciplines/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Disciplines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Course")] tblDisciplines tblDisciplines)
        {
            if (ModelState.IsValid)
            {
                db.tblDisciplines.Add(tblDisciplines);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tblDisciplines);
        }

        // GET: Disciplines/Edit/5
        public ActionResult Edit(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDisciplines tblDisciplines = db.tblDisciplines.Find(id);
            if (tblDisciplines == null)
            {
                return HttpNotFound();
            }
            return View(tblDisciplines);
        }

        // POST: Disciplines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Course")] tblDisciplines tblDisciplines)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblDisciplines).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tblDisciplines);
        }

        // GET: Disciplines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblDisciplines tblDisciplines = db.tblDisciplines.Find(id);
            if (tblDisciplines == null)
            {
                return HttpNotFound();
            }
            return View(tblDisciplines);
        }

        // POST: Disciplines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblDisciplines tblDisciplines = db.tblDisciplines.Find(id);
            db.tblDisciplines.Remove(tblDisciplines);
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
