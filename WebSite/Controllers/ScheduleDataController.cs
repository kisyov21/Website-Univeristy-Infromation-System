using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using GoogleApi1;
using GoogleApi1.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebSite;
using WebSite.Methods;
using WebSite.ViewModels.ScheduleViewModel;


namespace WebSite.Controllers
{
    public class ScheduleDataController : Controller
    {
        private WebSiteDBEntities db = new WebSiteDBEntities();
        private Crypto cr = new Crypto();

        // GET: tblScheduleDatas
        public ActionResult Index()
        {

            var tblScheduleData = db.tblScheduleData.Include(t => t.tblDisciplines).Include(t => t.tblTeachers);
            return View(tblScheduleData.ToList());
        }

        // GET: tblScheduleDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblScheduleData tblScheduleData = db.tblScheduleData.Find(id);
            if (tblScheduleData == null)
            {
                return HttpNotFound();
            }
            return View(tblScheduleData);
        }

        // GET: tblScheduleDatas/Create
        public ActionResult Create()
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 2)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            tblScheduleData model = new tblScheduleData();

            int id = int.Parse(cr.CryptoOrDecrypto(Session["TeacherID"].ToString(), false));
            var disciplines = db.tblDisciplines.ToList();
            var Ischecked = db.tblDisciplinesMapping.Where(p => p.TeacherID == id && p.IsActive >= 0).Select(p => p.DisciplineID).ToList();

            var oldInNew = from old in db.tblDisciplines   // mapping between tables 
                           join ID in Ischecked
                           on old.ID equals ID
                           select old;
            var result = oldInNew.ToList();

            ViewBag.Disciplines = result;
            ViewBag.TeacherID = id;
            return View(model);
        }

        // POST: tblScheduleDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(tblScheduleData data, string discipline)
        {
            int id = int.Parse(cr.CryptoOrDecrypto(Session["TeacherID"].ToString(), false));
            data.TeacherID = id;
            data.DisciplineID = Int32.Parse(discipline);

            if (ModelState.IsValid)
            {
                db.tblScheduleData.Add(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

          
            return View(data);
        }

        // GET: tblScheduleDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 2)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            if (id < 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblScheduleData data = db.tblScheduleData.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            ScheduleViewModel model = new ScheduleViewModel(data.ID, data.StartDate, data.EndDate, data.Type, data.Room, data.Topic, data.FilePath, data.TeacherID, data.DisciplineID);

            ViewBag.DisciplineID = new SelectList(db.tblDisciplines, "ID", "Name", model.DisciplineID);
            ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", model.TeacherID);
            return View(model);
        }

        // POST: tblScheduleDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScheduleViewModel model, HttpPostedFileBase file)   
        {
            if (ModelState.IsValid)
            {
                try
                {
                    tblScheduleData data = db.tblScheduleData.Find(model.ID);
                    GoogleDriveFiles newFile = new GoogleDriveFiles();
                    var title = "";
                    if (file != null)
                    {
                        var service = GoogleDrive.NewService();
                        var response = GoogleDrive.uploadFile(service, file);
                        title = response.Title;
                        newFile.GoogleDrive_ID = response.Id;
                        newFile.EventID = model.ID;
                        newFile.Title = response.Title;
                        newFile.DownloadURL = response.DownloadUrl;
                        newFile.FileSize = response.FileSize.ToString();
                        db.GoogleDriveFiles.Add(newFile);

                    }

                    data.FilePath = title;
                    data.StartDate = Convert.ToDateTime(model.start);
                    data.EndDate = Convert.ToDateTime(model.end);
                    data.Type = model.Type;
                    data.Room = model.Room;
                    data.Topic = model.Topic;

                    db.Entry(data).State = EntityState.Modified;
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
            return RedirectToAction("Index");
        }

        // GET: tblScheduleDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 2)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblScheduleData tblScheduleData = db.tblScheduleData.Find(id);
            if (tblScheduleData == null)
            {
                return HttpNotFound();
            }
            return View(tblScheduleData);
        }

        // POST: tblScheduleDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if ((int)Session["CurrentUserPermissionLevel"] != 2)
            {
                return View("~/Views/Shared/NoPermission.cshtml");
            }
            try
            {
                GoogleDriveFiles file = db.GoogleDriveFiles.Where(f => f.EventID == id).FirstOrDefault();
                db.GoogleDriveFiles.Remove(file);
                tblScheduleData tblScheduleData = db.tblScheduleData.Find(id);
                db.tblScheduleData.Remove(tblScheduleData);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return RedirectToAction("Index");
        }
        public ActionResult Calendar()
        {
            return View();
        } 

        public ActionResult Download(int eventID)
        {            
            bool result = false;
            if (eventID >=0)
            {
                try
                {
                    GoogleDriveFiles googleDriveFile = db.GoogleDriveFiles.Where(g => g.EventID == eventID).FirstOrDefault();
                    GoogleDriveFile file = new GoogleDriveFile(googleDriveFile.Title, googleDriveFile.GoogleDrive_ID, googleDriveFile.DownloadURL, googleDriveFile.FileSize);
                    string output = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    var service = GoogleDrive.NewService();
                    var response = GoogleDrive.downloadFile(service, file, output);
                   
                    result = true;
                    //TODO
                }
                catch (DbEntityValidationException ex)
                {
                    DateTime currentTime = DateTime.Now;
                    string _error = ex.ToString();
                    _error = currentTime + " Exception: " + _error;
                    var а = _error;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteFile(int eventID)
        {
            bool result = false;
            if (eventID >= 0)
            {
                try
                {
                    GoogleDriveFiles googleDriveFile = db.GoogleDriveFiles.Where(g => g.EventID == eventID).FirstOrDefault();
                    GoogleDriveFile file = new GoogleDriveFile(googleDriveFile.Title, googleDriveFile.GoogleDrive_ID, googleDriveFile.DownloadURL, googleDriveFile.FileSize);
                    string output = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                    var service = GoogleDrive.NewService();
                    var response = GoogleDrive.deleteFile(service, file);
                    db.GoogleDriveFiles.Remove(googleDriveFile);
                    tblScheduleData tblScheduleData = db.tblScheduleData.Find(eventID);
                    tblScheduleData.FilePath = "";
                    db.Entry(tblScheduleData).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ScheduleCalendar()
        {
            List<ScheduleViewModel> data = new List<ScheduleViewModel>();
            List<ScheduleViewModel> sortedData = new List<ScheduleViewModel>();
            var start = Request.QueryString["start"] == null || Request.QueryString["start"] == string.Empty ? DateTime.Now.AddMonths(-2) : Convert.ToDateTime(Request.QueryString["start"]);
            var end = Request.QueryString["end"] == null || Request.QueryString["end"] == string.Empty ? DateTime.Now : Convert.ToDateTime(Request.QueryString["end"]);
            var dataTemp = db.tblScheduleData
                .Where(x => x.StartDate >= start && x.EndDate <= end)
                .ToList();

            foreach (var item in dataTemp)
            {
                ScheduleViewModel temp = new ScheduleViewModel(item.ID,item.StartDate, item.EndDate, item.Type, item.Room, item.Topic, item.FilePath, item.TeacherID, item.DisciplineID);
                data.Add(temp);
            }
            if ((int)Session["CurrentUserPermissionLevel"] == 3)
            {
                int course = (int)Session["CurrentUserCourse"];
                List<ScheduleViewModel> temp = new List<ScheduleViewModel>();
                temp = data.Where(d => d.Course == course).ToList();
                sortedData = temp.OrderBy(o => o.start).ToList();
            }
            else if((int)Session["CurrentUserPermissionLevel"] == 2)
            {
                int id = int.Parse(cr.CryptoOrDecrypto(Session["CurrentUserID"].ToString(), false));
                List<ScheduleViewModel> temp = new List<ScheduleViewModel>();
                temp = data.Where(d => d.TeacherID == id).ToList();
                sortedData = data.OrderBy(o => o.start).ToList();
            }

            return Json(sortedData, JsonRequestBehavior.AllowGet);
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
