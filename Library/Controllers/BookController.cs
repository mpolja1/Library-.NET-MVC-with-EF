using DAL.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

        // GET: Book
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetBook()
        {
            using(db)
            {
                var bookList = db.Book.ToList<Book>();
                return Json(new {data=bookList},JsonRequestBehavior.AllowGet);
            }
        }
    }
}