using DAL.DBContext;
using Library.Models;
using PayPal.Api;
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

        private Payment payment;
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
                cartBooks = (List<Book>)Session["cart"];
                cartBooks.Add(book);
                ViewBag.cart = cartBooks.Count();
                Session["cartCount"] = Convert.ToInt32(Session["cartCount"]) + 1;

            }
            return RedirectToAction("Index", "Book");


        }
        [HttpGet]
        public ActionResult ShoppingCart()
        {

            return View((List<Book>)Session["cart"]);
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<Book> books)
        {

            return RedirectToAction("PaymentWithPaypal", "ShoppingCart");
        }

        public ActionResult RemoveItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            cartBooks = (List<Book>)Session["cart"];
            var book = cartBooks.Find(x => x.BookId == id);
            cartBooks.Remove(book);
            Session["cart"] = cartBooks;
            Session["cartCount"] = Convert.ToInt32(Session["cartCount"]) - 1;

            return RedirectToAction("ShoppingCart", "ShoppingCart");
        }

        [HttpGet]
        public ActionResult Borrow(int? id)
        {
            if (id == null)
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



            return RedirectToAction("Index", "Book");
        }

        //PayPal payment
        public Payment Placanje(APIContext aPIContext, string redirectUrl)
        {
            var listItems = new ItemList { items = new List<Item>() };
            //cartBooks = (List<Book>)Session["cart"];
            foreach (var book in cartBooks)
            {
                listItems.items.Add(new Item()
                {
                    name = book.Title,
                    currency = "USD",
                    price = book.BuyPrice.ToString(),
                    quantity = 1.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer() { payment_method = "paypal" };


            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = "109"
            };

            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),
                details = details
            };


            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction
            {
                description = "Online library",
                invoice_number = Convert.ToString((new Random()).Next(10000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(aPIContext);


        }
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymetnId)
        {
            var paymetExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymetnId };
            return payment.Execute(apiContext, paymetExecution);
        }
      
        public ActionResult PaymentWithPaypal()
        {
            APIContext aPIContext = PayPalConfiguration.GetApiContext();


           
                try
                {
                    string payerId = Request.Params["PayerID"];
                    if (string.IsNullOrEmpty(payerId))
                    {
                        string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority + "/ShoppingCart/PaymentWithPaypal?/";
                        var guid = Convert.ToString((new Random()).Next(10000));
                        var createPayment = Placanje(aPIContext, baseUri + "guide=" + guid);

                        var links = createPayment.links.GetEnumerator();
                        string paypalRedirectUrl = string.Empty;
                        while (links.MoveNext())
                        {
                            Links link = links.Current;
                            if (link.rel.ToLower().Trim().Equals("approval_url"))
                            {
                                paypalRedirectUrl = link.href;
                            }
                        }
                        Session.Add(guid, createPayment.id);

                        return Redirect(paypalRedirectUrl);


                    }
                    else
                    {
                        var guid = Request.Params["guid"];
                        var executePayment = ExecutePayment(aPIContext, payerId, Session[guid] as string);
                        if (executePayment.state.ToLower() != "approved")
                        {
                            return View("Success");
                        }
                    }
                }

                catch (Exception ex)
                {

                    return View("Failuer");
                }
            
            return View("Success");
        }
    }

}
