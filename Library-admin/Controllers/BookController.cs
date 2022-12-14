using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DAL.DBContext;
using Library_admin.Models.CustomAttributes;

namespace Library_admin.Controllers
{
    [IsAuthorized]
    public class BookController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

      

        //[IsAuthorized]
        public async Task<ActionResult> Index()
        {
            ViewBag.Poruka = "Do you want to delete?";
            var book = db.Book.Where(x => x.DeletedAt == null);
            return View(await book.ToListAsync());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

     
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "FirstName");
            ViewBag.PublisherId = new SelectList(db.Publisher, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Title,AuthorId,ISBN,BuyPrice,BorrowPrice,StockAvailabilty,CoverImage,Description,Condition,PublisherId,SoldNumber")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Book.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.PublisherId = new SelectList(db.Publisher, "Id", "Name", book.PublisherId);
            return View(book);
        }

   
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Book.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.PublisherId = new SelectList(db.Publisher, "Id", "Name", book.PublisherId);
           
            return View(book);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Title,AuthorId,ISBN,BuyPrice,BorrowPrice,StockAvailabilty,CoverImage,Description,Condition,PublisherId,SoldNumber")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "FirstName", book.AuthorId);
            ViewBag.PublisherId = new SelectList(db.Publisher, "Id", "Name", book.PublisherId);
            return View(book);
        }

        public JsonResult DeleteBook(int BookId)
        {

            bool result = false;
            Book book = db.Book.Find(BookId);
            
            if (book != null)
            {
                db.Book.Remove(book);
                db.SaveChanges();

                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Hide(int? id)
        {
            if (id != null)
            {
                db.HideBook(id);
            }


            return RedirectToAction("Index");
        }

       [HttpGet]
        public ActionResult AddAuthor()
        {
          
            return View();
        }

        [HttpPost]
        public ActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                db.Author.Add(author);
                db.SaveChanges();

                return RedirectToAction("Create");
            }
            

            return View();
        }

        [HttpGet]
        public ActionResult AddPublisher()
        {
            

            return View();
        }

        [HttpPost]
        public ActionResult AddPublisher(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Publisher.Add(publisher);
                db.SaveChanges();

                return RedirectToAction("Create");
            }

            return View();
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
