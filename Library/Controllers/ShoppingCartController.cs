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
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart

        private KnjiznicaEntities db = new KnjiznicaEntities();
        private List<Book> cartBooks = new List<Book>();
       
        public ActionResult AddItemToCart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var book = db.Book.Find(id);

            var user = Session["User"] as UserAsp;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["cart"] == null)
            {
                cartBooks.Add(book);
                Session["cart"] = cartBooks;
                ViewBag.cart = cartBooks.Count();

                Session["cartCount"] = 1;

            }
            else
            {
                cartBooks= (List<Book>)Session["cart"];
                cartBooks.Add(book);
                ViewBag.cart = cartBooks.Count();
                Session["cartCount"] = Convert.ToInt32(Session["cartCount"]) + 1;

            }
            return RedirectToAction("Index", "Book");
            

        }
        public ActionResult ShoppingCart()
        {
            
            return View((List<Book>)Session["cart"]);
        }

        public ActionResult RemoveItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            cartBooks = (List<Book>)Session["cart"];
            var book = cartBooks.Find(x=>x.BookId==id);
            cartBooks.Remove(book);
            Session["cart"] = cartBooks;
            Session["cartCount"] = Convert.ToInt32(Session["cartCount"]) - 1;
            
            return RedirectToAction("ShoppingCart","ShoppingCart");
        }

        [HttpGet]
        public ActionResult Borrow(int? id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }
            var book = db.Book.Find(id);

            return View(book);
        }

        [HttpPost]
        public ActionResult Borrow(LoanHistory loan)
        {
            var user = Session["User"] as UserAsp;
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            loan.UserId = user.UserId;
            loan.Borrowed = DateTime.Now;

            double bookPrice = (double)db.Book.Find(loan.BookId).BorrowPrice;

            db.LoanHistory.Add(loan);
            db.SaveChanges();
            double totalPrice = 0;
            totalPrice = (double)((loan.BorrowedUntil - loan.Borrowed).TotalDays * bookPrice);
            TempData["BookBorrow"] = $"You have successfully borrowed a book. The book is waiting for you in the bookshop. The total price is: {Convert.ToInt32(totalPrice)} kn";
          
            return RedirectToAction("Index","Book");
        }


        public ActionResult Paypal()
        {

            return View();
        }
       
    }

  

}