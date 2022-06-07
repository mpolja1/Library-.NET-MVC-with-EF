using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL.DBContext;

namespace Library_admin.Controllers
{
    public class LibraryController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

      
        public ActionResult Edit(int id=1)
        {
            
            Library library = db.Library.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Iban,Image")] Library library)
        {
            if (ModelState.IsValid)
            {
                db.Entry(library).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(library);
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
