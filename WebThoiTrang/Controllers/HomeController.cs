using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;
using Microsoft.AspNet.Identity;

namespace WebThoiTrang.Controllers
{

    public class HomeController : Controller
    {
        private CT25Team12Entities db = new CT25Team12Entities();
        private List<CartDetail> cart = null;

        private void GetCart()
        {
            if (Session["cart"] != null)
                cart = Session["cart"] as List<CartDetail>;
            else
            {
                cart = new List<CartDetail>();
                Session["cart"] = cart;
            }
        }
        public ActionResult Index()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                GetCart();
                if (ModelState.IsValid)
                {
                    string cartCode = "cart" + userId.Substring(0, 8);

                    var userCart = db.Carts.Find(cartCode);

                    cart = userCart.CartDetails.ToList();
                    Session["cart"] = cart;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            var products = db.Products;
            var listCoupons = db.Coupons.ToList();

            ViewBag.MAGIAMGIA = listCoupons;
            return View(products.ToList());
        }


        
    }
}