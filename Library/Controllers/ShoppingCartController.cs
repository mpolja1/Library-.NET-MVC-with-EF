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

        public ActionResult Borrow()
        {


            return View();
        }


        public ActionResult Paypal()
        {

            return View();
        }
       
    }

  

}