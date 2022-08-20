using DAL.DBContext;
using DAL.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            var authUser = db.UserAsp.Where(x => x.Email == user.Email && x.Password == user.Password && x.DeletedAt==null).FirstOrDefault();
            
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
        public ActionResult ForgotPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            string newPassword = Guid.NewGuid().ToString();
            newPassword = newPassword.Substring(0, 15);
            var user = db.UserAsp.Where(x => x.Email == email).FirstOrDefault();
           
            if (user != null)
            {
                try
                {
                    db.UserAsp.Attach(user).Password = newPassword;
                    db.Entry(user).Property(x => x.Password).IsModified = true;
                    db.SaveChanges();

                    EmailUtility.SendMail(email, "Forgot password", "This is your new password: " + newPassword, null);
                    ViewBag.ForgotPassword = "New password is sent on Email";
                }
                catch (Exception e)
                {

                    throw;
                }
 
            }
            else
            {
                ViewBag.ErrorForgotPassword = "Email doesn't exists";
            }

            
            return View();
        }
    }
}