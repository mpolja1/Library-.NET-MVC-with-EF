using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL.DBContext;

namespace Library.Controllers
{
    public class BookController : Controller
    {
        private KnjiznicaEntities db = new KnjiznicaEntities();

        // GET: Book
        public ActionResult Index(string search)
        {
            var book = db.Book.Include(b => b.Author).Include(b => b.Publisher);
            return View(book.ToList());
        }

        // GET: Book/Details/5
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

        // GET: Book/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Author, "AuthorId", "FirstName");
            ViewBag.PublisherId = new SelectList(db.Publisher, "Id", "Name");
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Book/Edit/5
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

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Book/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Book.Find(id);
            db.Book.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index");
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
