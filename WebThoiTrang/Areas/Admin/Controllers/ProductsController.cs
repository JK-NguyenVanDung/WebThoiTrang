using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebThoiTrang.Models;
namespace WebThoiTrang.Areas.Admin.Controllers
{
    [Authorize(Roles = "Nhân Viên")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private CT25Team12Entities db = new CT25Team12Entities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.LOHANG);
            return View(products.ToList());
        }


        // GET: Products/Details/5
        [AllowAnonymous]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOHANG = new SelectList(db.LOHANGs, "MALOHANG", "MANPP", product.MALOHANG);

            return View(product);
        }
        [AllowAnonymous]
        public ActionResult Search(string keyword)
        {
            var model = db.Products.Include(p => p.LOHANG).ToList();

            model = model.Where(p => p.TENSANPHAM.ToLower().Contains(keyword.ToLower())).ToList();
            return View("Search", model);
        }
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.MALOHANG = new SelectList(db.LOHANGs, "MALOHANG", "MANPP");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MASP,MALOHANG,TENSANPHAM,ANHSP,GIATIEN,THUONGHIEU,MANPP,SIZE,NGAYTRAVE,VAT,MAU,SOLUONG,MOTA,TENNV,LOAI,KIEU")] Product product)
        {
            product.TENNV = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MALOHANG = new SelectList(db.LOHANGs, "MALOHANG", "MANPP", product.MALOHANG);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.MALOHANG = new SelectList(db.LOHANGs, "MALOHANG", "MANPP", product.MALOHANG);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MASP,MALOHANG,TENSANPHAM,ANHSP,GIATIEN,THUONGHIEU,MANPP,SIZE,NGAYTRAVE,VAT,MAU,SOLUONG,MOTA,TENNV,LOAI,KIEU")] Product product)
        {
            product.TENNV = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MALOHANG = new SelectList(db.LOHANGs, "MALOHANG", "MANPP", product.MALOHANG);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
