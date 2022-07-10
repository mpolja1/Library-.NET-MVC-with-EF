using DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_admin.Controllers
{
    public class HomeController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

        [HttpGet]
        public ActionResult Login()
        {
  
            return View();
        }

        [HttpPost]
        public ActionResult Login(Employee employee)
        {
            var zaposlenik =  db.Employee.Where(x=> x.Email == employee.Email && x.Password==employee.Password).FirstOrDefault();
            if (zaposlenik!=null)
            {
                if (ModelState.IsValid)
                {
                    Session["Employee"] = zaposlenik;
                    return RedirectToAction("Index", "Book");
                }
                
            }
            else
            {
                ViewBag.Notification = "Wrong password/email";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction(actionName: "Login", controllerName: "Home");
        }

    }
}