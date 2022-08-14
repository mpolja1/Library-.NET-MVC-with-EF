using DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class UserProfileController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

        //[ActionName("Edit")]
        [HttpGet]
        
        public ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserAsp user = db.UserAsp.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        //[ActionName("Edit")]
        [HttpPost]
        public ActionResult Profile(UserAsp user)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(actionName:"Index",controllerName:"Book");
            }
         
            return View(user);
        }
        public ActionResult PurchasedBooks(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var purhhist = db.PurchasedHistory.Where(x => x.UserAsp.UserId == id);


            return View(purhhist);
        }
        public ActionResult Loan(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var loans = db.LoanHistory.Where(x => x.UserAsp.UserId == id);


            return View(loans);
        }
        public ActionResult DeleteAccount(int? idUser)
        {
            if (idUser == null)
            {
                return HttpNotFound();
            }
            var user = db.UserAsp.Find(idUser);

            db.UserAsp.Attach(user).DeletedAt=DateTime.Now;
            db.Entry(user).Property(x => x.DeletedAt).IsModified = true;
            db.SaveChanges();
            TempData["AccountDeleted"] = "Account deleted successfully";

            return RedirectToAction("Logout", "Account");
        }
    }
}