using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebSite;
using WebSite.Methods;
using WebSite.ViewModels.TeachersViewModel;

namespace WebSite.Controllers
{
    public class TeachersController : Controller
    {
        private MethodsDB mdb = new MethodsDB();
        private WebSiteDBEntities db = new WebSiteDBEntities();
        private Crypto cr = new Crypto();

        // GET: tblTeachers
        public ActionResult Index()
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            return View(db.tblTeachers.ToList());
        }

        // GET: tblTeachers/Details/5
        public ActionResult Details(int? id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 1)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTeachers tblTeachers = db.tblTeachers.Find(id);
            if (tblTeachers == null)
            {
                return HttpNotFound();
            }
            return View(tblTeachers);
        }

        // GET: tblTeachers/Create
        public ActionResult Create()
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 2)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            TeachersViewModel model = new TeachersViewModel();
            return View(model);
        }

        // POST: tblTeachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TeachersViewModel teacher, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                byte[] imageBytes = null;
                if (file != null)
                {
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageBytes = reader.ReadBytes((int)file.ContentLength);
                }
                tblTeachers _teacher = new tblTeachers() { FirstName = teacher.FirstName, LastName = teacher.LastName, Department = teacher.Department, Courses = teacher.Courses, Education = teacher.Education, Email = teacher.Email, PersonalCabinet = teacher.PersonalCabinet, PhoneNumber = teacher.PhoneNumber, ProfilePicture = imageBytes, ScientificInterests = teacher.ScientificInterests, VisitingHours = teacher.VisitingHours };
                try
                {
                    var result = cr.CryptoOrDecrypto(Session["CurrentUserID"].ToString(), false);
                    int currentUserID = int.Parse(result);
                    db.Entry(_teacher).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();
                    AddTeacherID(currentUserID, _teacher.ID);
                    return RedirectToAction("Index");
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

        public void AddTeacherID(int loginId, int teacherId)
        {
            tblLogin _login = new tblLogin() { ID = loginId, TeacherID = teacherId };
            try
            {
                db.tblLogin.Attach(_login);
                db.Entry(_login).Property(X => X.TeacherID).IsModified = true;
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                Session["TeacherID"] = cr.CryptoOrDecrypto(teacherId.ToString(), true);
            }
            catch (DbEntityValidationException ex)
            {
                DateTime currentTime = DateTime.Now;
                string _error = ex.ToString();
                _error = currentTime + " Exception: " + _error;
                var а = _error;
            }
        }

        // GET: tblTeachers/Edit/5
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Edit()
        {
            int id = -1;
            var result = Session["TeacherID"];
            if (result != null)
            {
                id = int.Parse(cr.CryptoOrDecrypto(Session["TeacherID"].ToString(), false));
            }
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblTeachers tblTeachers = db.tblTeachers.Find(id);
            if (tblTeachers == null)
            {
                return HttpNotFound();
            }
            TeachersViewModel model = new TeachersViewModel() { ID = tblTeachers.ID, FirstName = tblTeachers.FirstName, LastName = tblTeachers.LastName, Department = tblTeachers.Department, Courses = tblTeachers.Courses, Education = tblTeachers.Education, Email = tblTeachers.Email, PersonalCabinet = tblTeachers.PersonalCabinet, PhoneNumber = tblTeachers.PhoneNumber, ScientificInterests = tblTeachers.ScientificInterests, VisitingHours = tblTeachers.VisitingHours };

            return View(model);
        }

        // POST: tblTeachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TeachersViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    byte[] imageBytes = null;
                    tblTeachers teacher = db.tblTeachers.Find(model.ID);

                    if (file != null)
                    {
                        BinaryReader reader = new BinaryReader(file.InputStream);
                        imageBytes = reader.ReadBytes((int)file.ContentLength);
                        teacher.ProfilePicture = imageBytes;
                    }
                    teacher.FirstName = model.FirstName;
                    teacher.LastName = model.LastName;
                    teacher.Department = model.Department;
                    teacher.Courses = model.Courses;
                    teacher.Education = model.Education;
                    teacher.ScientificInterests = model.ScientificInterests;
                    teacher.Email = model.Email;
                    teacher.VisitingHours = model.VisitingHours;
                    teacher.PhoneNumber = model.PhoneNumber;
                    teacher.PersonalCabinet = model.PersonalCabinet;

                    db.Entry(teacher).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    DateTime currentTime = DateTime.Now;
                    string _error = ex.ToString();
                    _error = currentTime + " Exception: " + _error;
                    var а = _error;
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult GetImage(int? id)
        {
            byte[] imagedata;
            int _id = id ?? -100;

            if (_id > 0)
            {
                tblTeachers teacher = db.tblTeachers.Find(id);
                imagedata = teacher.ProfilePicture;
                if (imagedata == null)
                {
                    imagedata = GetDefaultImg();
                }
            }
            else
            {
                imagedata = GetDefaultImg();
            }
            return File(imagedata, "image/jpg");
        }

        public byte[] GetDefaultImg()
        {
            String fileName = Server.MapPath(Url.Content("~/Content/Images/defaultProfilePicture.png"));
            byte[] img = System.IO.File.ReadAllBytes(fileName);
            return img;
        }

        // GET: tblTeachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblTeachers tblTeachers = db.tblTeachers.Find(id);
            if (tblTeachers == null)
            {
                return HttpNotFound();
            }
            return View(tblTeachers);
        }

        // POST: tblTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                tblTeachers tblTeachers = db.tblTeachers.Find(id);
                db.tblTeachers.Remove(tblTeachers);
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                DateTime currentTime = DateTime.Now;
                string _error = ex.ToString();
                _error = currentTime + " Exception: " + _error;
                var а = _error;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Disciplines()
        {
            int id = int.Parse(cr.CryptoOrDecrypto(Session["TeacherID"].ToString(), false));
            var disciplines = db.tblDisciplines.ToList();
            var Ischecked = db.tblDisciplinesMapping.Where(p => p.TeacherID == id && p.IsActive >= 0).Select(p => p.DisciplineID).ToArray();
            ViewBag.Checked = Ischecked;
            ViewBag.disciplines = disciplines;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disciplines(int[] disciplines)
        {
            var _disciplines = db.tblDisciplines.ToList();
            ViewBag.disciplines = _disciplines;
            int id = int.Parse(cr.CryptoOrDecrypto(Session["TeacherID"].ToString(), false));
            try
            {
                if (disciplines == null)
                { //maha disciplinite 
                    var activeDisc = db.tblDisciplinesMapping.Where(p => p.TeacherID == id && p.IsActive >= 0).Select(p => p.ID).ToArray();
                    for (int i = 0; i < activeDisc.Length; i++)
                    {
                        tblDisciplinesMapping temp = db.tblDisciplinesMapping.Find(activeDisc[i]);

                        temp.IsActive = -1;
                        db.Entry(temp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    var activeDisc = db.tblDisciplinesMapping.Where(p => p.TeacherID == id && p.IsActive >= 0).Select(p => p.ID).ToArray();
                    for (int i = 0; i < activeDisc.Length; i++)
                    {
                        tblDisciplinesMapping temp = db.tblDisciplinesMapping.Find(activeDisc[i]);

                        temp.IsActive = -1;
                        db.Entry(temp).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    for (int i = 0; i < disciplines.Length; i++)
                    {
                        var doExist = db.tblDisciplinesMapping.AsEnumerable().Where(p => p.TeacherID == id && p.DisciplineID == disciplines[i]).Select(p => p.ID).ToList();
                        if (doExist.Count != 0)
                        {
                            for (int k = 0; k < doExist.Count; k++)
                            {
                                tblDisciplinesMapping temp = db.tblDisciplinesMapping.Find(doExist[k]);
                                if (temp != null)
                                {
                                    if (temp.IsActive < 0 || temp.IsActive == null)
                                    {
                                        temp.IsActive = 1;
                                        db.Entry(temp).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (db)
                            {
                                tblDisciplinesMapping mapping = new tblDisciplinesMapping
                                {
                                    TeacherID = id,
                                    DisciplineID = disciplines[i],
                                    IsActive = 1
                                };
                                db.tblDisciplinesMapping.Add(mapping);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                DateTime currentTime = DateTime.Now;
                string _error = ex.ToString();
                _error = currentTime + " Exception: " + _error;
                var а = _error;
            }
            catch (Exception ex)
            { var error = ex; }
            return RedirectToAction("Edit");
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
