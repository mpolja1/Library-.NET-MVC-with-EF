using DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserAsp user)
        {
            var authUser = db.UserAsp.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            
            if (authUser != null)
            {

                    Session["User"] = authUser;
                    return RedirectToAction("Index", controllerName: "Book");
                
            }
            else
            {
                ViewBag.Notification = "Wrong password or Email";
            }
            return View();
        }
        public ActionResult Registration()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(UserAsp user)
        {
            if (ModelState.IsValid)
            {
                var email = db.UserAsp.Where(x => x.Email == user.Email).FirstOrDefault();
                if (email==null)
                {
                    db.UserAsp.Add(user);
                    db.SaveChanges();
                    TempData["SuccessRegistration"] = "Uspjesna registracija";
                    return RedirectToAction("Index", controllerName: "Book");
                }
                else
                {
                    ViewBag.EmailExists = "Email already exists";
                }
           
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("index", "Book");
        }

    }
}