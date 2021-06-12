using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;
using Microsoft.AspNet.Identity;

namespace WebThoiTrang.Controllers
{
    public class CartController : Controller
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

        // GET: Cart
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            GetCart();
            string cartCode = "";
            if (userId != null)
            {

                cartCode = "cart" + userId.Substring(0, 8);

                Cart usercart = new Cart();

                usercart.NGAYTAO = DateTime.Now.Date;
                usercart.MAGIOHANG = cartCode;
                usercart.MAKH = userId.Substring(0, 8);
                if (ModelState.IsValid && db.Carts.Find(cartCode) == null)
                {
                    db.Carts.Add(usercart);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                else if (ModelState.IsValid)
                {
                    var userCart = db.Carts.Find(cartCode);

                    cart = userCart.CartDetails.ToList();
                    Session["cart"] = cart;
                }

            }


            var hashtable = new Hashtable();
            foreach (var CartDetail in cart)
            {
                if (hashtable[CartDetail.Product.MASP] != null)
                {
                    (hashtable[CartDetail.Product.MASP] as CartDetail).SOLUONG += CartDetail.SOLUONG;
                }
                else hashtable[CartDetail.Product.MASP] = CartDetail;
            }
            cart.Clear();
            foreach (CartDetail CartDetail in hashtable.Values)
                cart.Add(CartDetail);
            ViewBag.MAGIAMGIA = new SelectList(db.Coupons, "MAMGGIA", "MANV");

            try
            {
                string magiamgia = db.Carts.Find(cartCode).MAGIAMGIA;
                ViewData["USERCOUPONS"] = magiamgia;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(cart);
        }



        // GET: Cart/Create
        [HttpPost]
        public ActionResult Create(string productId, int Quantity)
        {
            GetCart();
            string userId = "";
            try
            {
                userId = User.Identity.GetUserId().Substring(0, 8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var product = db.Products.Find(productId);
            cart.Add(new CartDetail
            {
                MAGH = "cart" + userId,
                MASP = product.MASP,
                GIA = product.GIATIEN,
                Product = product,
                SOLUONG = Quantity
            });
            var newestItem = cart[cart.Count - 1];
            try
            {
                var sameProduct = db.CartDetails.SingleOrDefault(c => c.MASP == newestItem.MASP);
                if (sameProduct != null)
                {
                    sameProduct.SOLUONG += newestItem.SOLUONG;
                    db.SaveChanges();
                }
                else
                {
                    db.CartDetails.Add(newestItem);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return RedirectToAction("Index");
        }


        // GET: Cart/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartDetail CartDetail = db.CartDetails.Find(id);
            if (CartDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAGH = new SelectList(db.Carts, "MAHOADON", "MANV", CartDetail.MAGH);
            ViewBag.MASP = new SelectList(db.Products, "MASP", "MALOHANG", CartDetail.MASP);
            return View(CartDetail);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete()
        {
            GetCart();
            cart.Clear();
            Session["cart"] = cart;
            try
            {
                var cartList = db.CartDetails.ToList();
                foreach (var cart in cartList)
                {
                    db.CartDetails.Remove(cart);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Remove(string productId)
        {
            GetCart();
            CartDetail removedItem = cart.First(p => p.Product.MASP == productId);
            cart.Remove(removedItem);

            Session["cart"] = cart;
            try
            {
                var cartItm = db.CartDetails.First(p => p.Product.MASP == productId);

                db.CartDetails.Remove(cartItm);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index");
        }
        public ActionResult Update(string productId, int Quantity)
        {
            GetCart();
            string userId = "";
            try
            {
                userId = User.Identity.GetUserId().Substring(0, 8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            var product = db.Products.Find(productId);
            cart.Add(new CartDetail
            {
                MAGH = "cart" + userId,
                MASP = product.MASP,
                GIA = product.GIATIEN,
                Product = product,
                SOLUONG = Quantity
            });
            var newestItem = cart[cart.Count - 1];
            try
            {
                var sameProduct = db.CartDetails.SingleOrDefault(c => c.MASP == newestItem.MASP);
                if (sameProduct != null)
                {
                    sameProduct.SOLUONG = newestItem.SOLUONG;
                    db.SaveChanges();
                }
                else
                {
                    db.CartDetails.Add(newestItem);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


            return RedirectToAction("Index");

        }
        [Authorize]
        [HttpGet]
        public ActionResult GetCoupon(string Discount)
        {
            var userId = User.Identity.GetUserId();

            string cartCode = "cart" + userId.Substring(0, 8);
            if (db.Carts.Find(cartCode).MAGIAMGIA != Discount)
            {
                db.Carts.Find(cartCode).MAGIAMGIA += (Discount + ",");
                db.SaveChanges();
            }


            return RedirectToAction("index");
        }


        public ActionResult ApplyCode(string Discount)
        {
            var coupons = db.Coupons.ToList();
            foreach (var coupon in coupons)
            {
                if (coupon.MAMGGIA == Discount)
                {
                    TempData["couponValues"] = coupon.GIATRIGIAMGIATOIDA;
                    break;
                }
            }
            return RedirectToAction("Index");
        }


        //[Authorize]

        //public ActionResult GetCoupon(string Discount)
        //{
        //    var userId = User.Identity.GetUserId();

        //    string cartCode = "cart" + userId.Substring(0, 8);
        //    if (!db.Carts.Find(cartCode).MAGIAMGIA.Contains(Discount))
        //    {
        //        db.Carts.Find(cartCode).MAGIAMGIA += (Discount + ",");
        //        db.SaveChanges();
        //    }


        //    return RedirectToAction("index");
        //}

        //[Authorize]
        //public ActionResult ApplyCode(string Discount)
        //{
        //    var userId = User.Identity.GetUserId();

        //    string cartCode = "cart" + userId.Substring(0, 8);
        //    var userCart = db.Carts.Find(cartCode);
        //    if (userCart.MAGIAMGIA == Discount && !db.Carts.Find(cartCode).MAGIAMGIADAAPDUNG.Contains(Discount))
        //    {
        //        var coupon = db.Coupons.Find(Discount);
        //        TempData["couponValues"] = coupon.GIATRIGIAMGIATOIDA;
        //        db.Carts.Find(cartCode).MAGIAMGIA.Replace(Discount + ",", "");
        //        db.Carts.Find(cartCode).MAGIAMGIADAAPDUNG = Discount;
        //    }
        //    return RedirectToAction("Index");
        //}
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
