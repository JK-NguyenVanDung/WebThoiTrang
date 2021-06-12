using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class BillsController : Controller
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
        // GET: Bills
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var bills = db.Bills.ToList();
            return View(bills);
        }

        // GET: Bills/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            var details = db.BillDetails.ToList();
            List<BillDetail> billDetails = new List<BillDetail>();
            foreach (var detail in details)
            {
                if (bill.MAHOADON == detail.MAHD)
                {
                    billDetails.Add(detail);
                }
            }
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.billDetails = billDetails;
            return View(bill);
        }

        // GET: Bills/Create
        [Authorize]
        public ActionResult Create(string total)
        {
            GetCart();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");

            }
            ViewBag.Cart = cart;
            ViewBag.MAGIAMGIA = new SelectList(db.Coupons, "MAMGGIA", "MANV");
            ViewData["total"] = total;
            return View();
        }

        // POST: Bills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MAHOADON,MAKH,MAGIOHANG,GIATHANHTOAN,NGAYTHANHTOAN,Phone,Address,FirstName,LastName,Id,DiscountValue")] Bill bill, int id = 0)
        {

            GetCart();
            var userId = User.Identity.GetUserId();
            var lastBill = db.Bills.OrderByDescending(c => c.Id).FirstOrDefault();
            ValidateBill(bill);
            if (ModelState.IsValid)
            {
                if (id != 0)
                {
                    bill = db.Bills.Where(x => x.Id == id).FirstOrDefault<Bill>();

                }
                else if (lastBill == null)
                {
                    bill.MAHOADON = "HOADON001";
                }
                else
                {
                    bill.MAHOADON = "HOADON" + (Convert.ToInt32(lastBill.MAHOADON.Substring(7, lastBill.MAHOADON.Length - 7)) + 1).ToString("D3");
                }
                
                bill.MAKH = userId.ToString();
                bill.NGAYTHANHTOAN = DateTime.Now.Date;
                var cartDetailId = "CTHD" + (Convert.ToInt32(bill.MAHOADON.Substring(7, bill.MAHOADON.Length - 7)) + 1).ToString("D3");
                bill.MAGIOHANG = cartDetailId;
                db.Bills.Add(bill);

                foreach (var item in cart)
                {
                    db.BillDetails.Add(new BillDetail
                    {
                        MACTHD = bill.MAHOADON,
                        MAHD = bill.MAHOADON,
                        MASP = item.Product.MASP,
                        GIA = item.Product.GIATIEN,
                        SOLUONG = item.SOLUONG

                    });
                }
                db.SaveChanges();
                GetCart();
                cart.Clear();
                Session["cart"] = cart;

                var cartList = db.CartDetails.ToList();
                foreach (var cart in cartList)
                {
                    db.CartDetails.Remove(cart);
                    db.SaveChanges();
                }

                Session["cart"] = null;

                return RedirectToAction("OrderSuccess", "Bills",bill);
            }
            GetCart();
            ViewBag.Cart = cart;
            return View(bill);
        }
        private void ValidateBill(Bill model)
        {
            var regex = new Regex("[0-9]{3}");
            GetCart();

            if (!regex.IsMatch(model.Phone.ToString()))
            {
                ModelState.AddModelError("Phone", "Wrong Phone Number");

            }
        }

        public ActionResult OrderSuccess(Bill model)
        {
            ViewBag.BillDetails = db.BillDetails.ToList();
            return View(model);
        }
        // GET: Bills/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.MAGIAMGIA = new SelectList(db.Coupons, "MAMGGIA");
            return View(bill);
        }

        // POST: Bills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MAHOADON,MANV,MAKH,MAGIOHANG,GIATHANHTOAN,NGAYTHANHTOAN,MAGIAMGIA,Phone,Address,FirstName,LastName,Id")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MAGIAMGIA = new SelectList(db.Coupons, "MAMGGIA");
            return View(bill);
        }

        // GET: Bills/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }

            return View(bill);
        }

        // POST: Bills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Bill bill = db.Bills.Find(id);
            try
            {
                var billList = db.BillDetails.ToList();
                foreach (var detail in billList)
                {
                    if (detail.MAHD == bill.MAHOADON)
                    {
                        db.BillDetails.Remove(detail);
                        db.SaveChanges();
                    }
                }
                db.Bills.Remove(bill);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

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
