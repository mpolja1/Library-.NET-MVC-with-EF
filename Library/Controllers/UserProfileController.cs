using DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class UserProfileController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

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
    }
}