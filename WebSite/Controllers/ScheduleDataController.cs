using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

            //ViewBag.DisciplineID = new SelectList(db.tblDisciplines, "ID", "Name", tblScheduleData.DisciplineID);
            //ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", tblScheduleData.TeacherID);
            return View(data);
        }

        // GET: tblScheduleDatas/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.DisciplineID = new SelectList(db.tblDisciplines, "ID", "Name", tblScheduleData.DisciplineID);
            ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", tblScheduleData.TeacherID);
            return View(tblScheduleData);
        }

        // POST: tblScheduleDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StartDate,EndDate,Type,Room,Topic,FilePath,TeacherID,DisciplineID")] tblScheduleData tblScheduleData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblScheduleData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DisciplineID = new SelectList(db.tblDisciplines, "ID", "Name", tblScheduleData.DisciplineID);
            ViewBag.TeacherID = new SelectList(db.tblTeachers, "ID", "FirstName", tblScheduleData.TeacherID);
            return View(tblScheduleData);
        }

        // GET: tblScheduleDatas/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: tblScheduleDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tblScheduleData tblScheduleData = db.tblScheduleData.Find(id);
            db.tblScheduleData.Remove(tblScheduleData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Calendar()
        {
            return View();
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
                ScheduleViewModel temp = new ScheduleViewModel(item.StartDate, item.EndDate, item.Type, item.Room, item.Topic, item.FilePath, item.TeacherID, item.DisciplineID);
                data.Add(temp);
            }
            sortedData = data.OrderBy(o => o.start).ToList();
            return Json(sortedData, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public async Task<object> IndexAsync(CancellationToken cancellationToken)
        //{
        //    var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
        //        AuthorizeAsync(cancellationToken);

        //    if (result.Credential != null)
        //    {
        //        var service = new DriveService(new BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = result.Credential,
        //            ApplicationName = "WebSite"
        //        });

        //        // YOUR CODE SHOULD BE HERE..
        //        // SAMPLE CODE:
        //        var list = await service.Files.List().ExecuteAsync();
        //        ViewBag.Message = "FILE COUNT IS: " + list.Items.Count();
        //        return View();
        //    }
        //    else
        //    {
        //        return new RedirectResult(result.RedirectUri);
        //    }
        //}
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
