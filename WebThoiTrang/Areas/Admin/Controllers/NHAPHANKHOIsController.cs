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
    public class NHAPHANKHOIsController : Controller
    {
        private CT25Team12Entities db = new CT25Team12Entities();

        // GET: Admin/NHAPHANKHOIs
        public ActionResult Index()
        {
            return View(db.NHAPHANKHOIs.ToList());
        }

        // GET: Admin/NHAPHANKHOIs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANKHOI nHAPHANKHOI = db.NHAPHANKHOIs.Find(id);
            if (nHAPHANKHOI == null)
            {
                return HttpNotFound();
            }
            return View(nHAPHANKHOI);
        }

        // GET: Admin/NHAPHANKHOIs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/NHAPHANKHOIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANPP,QUAN,DUONG,THANHPHO,SONHA,EMAIL,SDT")] NHAPHANKHOI nhaphankhoi)
        {
            if (ModelState.IsValid)
            {
                db.NHAPHANKHOIs.Add(nhaphankhoi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhaphankhoi);
        }

        // GET: Admin/NHAPHANKHOIs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANKHOI nhaphankhoi = db.NHAPHANKHOIs.Find(id);
            if (nhaphankhoi == null)
            {
                return HttpNotFound();
            }
            return View(nhaphankhoi);
        }

        // POST: Admin/NHAPHANKHOIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANPP,QUAN,DUONG,THANHPHO,SONHA,EMAIL,SDT")] NHAPHANKHOI nhaphankhoi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhaphankhoi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhaphankhoi);
        }

        // GET: Admin/NHAPHANKHOIs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHAPHANKHOI nhaphankhoi = db.NHAPHANKHOIs.Find(id);
            if (nhaphankhoi == null)
            {
                return HttpNotFound();
            }
            return View(nhaphankhoi);
        }

        // POST: Admin/NHAPHANKHOIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NHAPHANKHOI nhaphankhoi = db.NHAPHANKHOIs.Find(id);
            db.NHAPHANKHOIs.Remove(nhaphankhoi);
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
