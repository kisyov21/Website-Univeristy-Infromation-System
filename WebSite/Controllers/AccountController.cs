using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WebSite.Models;
using WebSite.Methods;
using System.Security.Cryptography;
using System.Web;
using Microsoft.Owin.Security;

namespace WebSite.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (WebSiteDBEntities db = new WebSiteDBEntities())
            {
                return View(db.tblLogin.ToList());
            }
        }
        private MethodsDB mdb = new MethodsDB();
        private Crypto cr = new Crypto();
        //private ApplicationSignInManager _signInManager;
        //private ApplicationUserManager _userManager;

        public AccountController()
        {

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ID = "";
            using (WebSiteDBEntities db = new WebSiteDBEntities())
            {
                if (mdb.CheckDBConnection() != true)
                {
                    return View("~/Views/Error/NoDBConnection.cshtml");
                }

                var login = db.tblLogin.Single(u => u.LoginName == model.Login);
                if (login == null)
                {
                    ModelState.AddModelError("", "Invalid login name.");
                }

           

                using (MD5 md5Hash = MD5.Create())
                {
                    //var pass = cr.GetMd5Hash(md5Hash, model.Password); za sega ne raboti
                    //var userToLogin = db.tblLogin.Where(u => u.LoginName == model.Login && u.Password == pass).FirstOrDefault();
                    var userToLogin = db.tblLogin.Where(u => u.LoginName == model.Login && u.Password == model.Password).FirstOrDefault();
                    if (userToLogin == null)
                    {
                        ModelState.AddModelError("", "Invalid login name or password");
                        return View();
                    }

                    Session["CurrentUserID"] = cr.CryptoOrDecrypto(userToLogin.ID.ToString(), true);
                    Session["CurrentUserLogin"] = cr.CryptoOrDecrypto(userToLogin.LoginName.ToString(), true);
                    Session["CurrentUserPermissionLevel"] = userToLogin.PermissionLevel;
                    Session["CurrentUserCourse"] = userToLogin.Course;

                    if (userToLogin.TeacherID == null)
                    {
                        Session["TeacherID"] = null;
                    }
                    else
                    {
                        Session["TeacherID"] = cr.CryptoOrDecrypto(userToLogin.TeacherID.ToString(), true);
                        var teacher = db.tblTeachers.Where(t => t.ID == userToLogin.TeacherID).FirstOrDefault();
                        Session["FirstName"] = cr.CryptoOrDecrypto(teacher.FirstName.ToString(), true);
                        ViewBag.ID = teacher.ID;
                    }
                    return RedirectToAction("LoggedIn");
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoggedIn()
        {

            int state = CheckSessionState();

            if (state == 0)
            {
                return View();
                //return View("~/Views/Error/LoginNeeded.cshtml");
            }
            else
            {
                int type = (int)Session["CurrentUserPermissionLevel"];

                if (type == 1) //admin login
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (type == 2) // teacher
                {
                    if (Session["TeacherID"] == null)
                    {
                        return RedirectToAction("Create", "Teachers");
                    }
                    ViewBag.Name = cr.CryptoOrDecrypto(Session["FirstName"].ToString(), false);
                    ViewBag.Course = "";
                    return RedirectToAction("Index", "Home");
                }
                else if (type == 3) //student
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Login");
        }

        public int CheckSessionState()
        {
            if (Session["CurrentUserID"] != null)
            {
                var result = cr.CryptoOrDecrypto(Session["CurrentUserID"].ToString(), false);

                return int.Parse(result);
                //if (result != null)
                //{
                //    return int.Parse(result);
                //}
            }
            else return 0; // no session = not logged in
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
       
    }
}
