using DAL.DBContext;
using System;
using System.Collections.Generic;
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
            cartBooks.Add(book);


            if (Session["cart"] == null)
            {
                cartBooks.Add(book);
                Session["cart"] = cartBooks;
                ViewBag.cart = cartBooks.Count();

                Session["cartCount"] = 1;

            }
            else
            {
                cartBooks.Add(book);
                Session["cart"] = cartBooks;
                ViewBag.cart = cartBooks.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;

            }
            return RedirectToAction("Index", "Book");

        }
        public ActionResult ShoppingCart()
        {
            var book = db.Book;
            return View(book.ToList());
        }
    }

  

}