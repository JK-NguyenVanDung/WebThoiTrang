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
    public class LOHANGsController : Controller
    {
        private CT25Team12Entities db = new CT25Team12Entities();

        // GET: Admin/LOHANGs
        public ActionResult Index()
        {
            var lOHANGs = db.LOHANGs.Include(l => l.NHAPHANKHOI);
            return View(lOHANGs.ToList());
        }

        // GET: Admin/LOHANGs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOHANG lohang = db.LOHANGs.Find(id);
            if (lohang == null)
            {
                return HttpNotFound();
            }
            return View(lohang);
        }

        // GET: Admin/LOHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MANPP = new SelectList(db.NHAPHANKHOIs, "MANPP", "QUAN");
            return View();
        }

        // POST: Admin/LOHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MALOHANG,NGAYNHAP,MANPP")] LOHANG lohang)
        {
            lohang.NGAYNHAP = DateTime.Now.Date;
            if (ModelState.IsValid)
            {
                db.LOHANGs.Add(lohang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MANPP = new SelectList(db.NHAPHANKHOIs, "MANPP", "QUAN", lohang.MANPP);
            return View(lohang);
        }

        // GET: Admin/LOHANGs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOHANG lohang = db.LOHANGs.Find(id);
            if (lohang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MANPP = new SelectList(db.NHAPHANKHOIs, "MANPP", "QUAN", lohang.MANPP);
            return View(lohang);
        }

        // POST: Admin/LOHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MALOHANG,NGAYNHAP,MANPP")] LOHANG lohang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lohang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MANPP = new SelectList(db.NHAPHANKHOIs, "MANPP", "QUAN", lohang.MANPP);
            return View(lohang);
        }

        // GET: Admin/LOHANGs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOHANG lohang = db.LOHANGs.Find(id);
            if (lohang == null)
            {
                return HttpNotFound();
            }
            return View(lohang);
        }

        // POST: Admin/LOHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOHANG lohang = db.LOHANGs.Find(id);
            db.LOHANGs.Remove(lohang);
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
