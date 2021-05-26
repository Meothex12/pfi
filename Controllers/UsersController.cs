using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pfi.Models;

namespace pfi.Controllers
{
    public class UsersController : Controller
    {
        private Database1Entities DB = new Database1Entities();
        private DateTime OnLineUsersLastUpdate
        {
            get
            {
                if (Session["OnLineUsersUpdate"] == null)
                    Session["OnLineUsersUpdate"] = new DateTime(0);
                return (DateTime)Session["OnLineUsersUpdate"];
            }
            set
            {
                Session["OnLineUsersUpdate"] = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }

        /*--INSCRIPTION D'UN USAGER AU SITE--*/
        public ActionResult Subscribe()
        {
            return View(new UserView());
        }

        [HttpPost]
        public ActionResult Subscribe(UserView userView)
        {
            //if (ModelState.IsValid)
            //{
                if (DB.UserNameExist(userView.UserName))
                {
                    ModelState.AddModelError("UserName", "Ce nom d'usager existe déjà.");
                    return View(userView);
                }
                userView.Password = userView.NewPassword;
                DB.AddUser(userView);
                return RedirectToAction("Login");
            //}
            //return View(userView);
        }

        /*--CONNEXION D'UN USAGER AU SITE--*/
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                User userFound = DB.FindByUserName(loginView.UserName);
                if (userFound != null)
                {
                    if (userFound.Password != loginView.Password)
                    {
                        ModelState.AddModelError("Password", "Mot de passe incorrect");
                        return View(loginView);
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "Ce non d'usager n'existe pas");
                    return View(loginView);
                }
                OnlineUsers.AddSessionUser(userFound.ToUserView());
            }
            return RedirectToAction("../Films/Index");
        }

        public ActionResult Logout()
        {
            OnlineUsers.RemoveSessionUser();
            return RedirectToAction("Login");
        }

        [UserAccess]
        public ActionResult Profil()
        {
            UserView userView = OnlineUsers.CurrentUser;
            ViewBag.PasswordChangeToken = Guid.NewGuid().ToString().Substring(0, 8);
            return View(userView);
        }

        [HttpPost]
        public ActionResult Profil(UserView userview)
        {
            //if (ModelState.IsValid)
            //{
                string PasswordChangeToken = (string)Request["PasswordChangeToken"];
                if (userview.NewPassword.Equals(PasswordChangeToken))
                {
                    User user = DB.Users.Find(userview.Id);
                    userview.Password = user.Password;
                }
                else
                {
                    userview.Password = userview.NewPassword;
                }
                DB.UpdateUser(userview);
                userview.CopyToUserView(OnlineUsers.CurrentUser);
                OnlineUsers.LastUpdate = DateTime.Now;
            //}
            return RedirectToAction("../Films/Index");
        }
    }
}