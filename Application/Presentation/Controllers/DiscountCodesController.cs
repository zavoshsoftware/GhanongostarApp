using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class DiscountCodesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.DiscountCodes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountCode discountCode = db.DiscountCodes.Find(id);
            if (discountCode == null)
            {
                return HttpNotFound();
            }
            return View(discountCode);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,ExpireDate,IsPercent,Amount,IsMultiUsing,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DiscountCode discountCode)
        {
            if (ModelState.IsValid)
            {
				discountCode.IsDeleted=false;
				discountCode.CreationDate= DateTime.Now; 
                discountCode.Id = Guid.NewGuid();
                db.DiscountCodes.Add(discountCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discountCode);
        }

        // GET: DiscountCodes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountCode discountCode = db.DiscountCodes.Find(id);
            if (discountCode == null)
            {
                return HttpNotFound();
            }
            return View(discountCode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,ExpireDate,IsPercent,Amount,IsMultiUsing,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] DiscountCode discountCode)
        {
            if (ModelState.IsValid)
            {
				discountCode.IsDeleted=false;
                db.Entry(discountCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discountCode);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountCode discountCode = db.DiscountCodes.Find(id);
            if (discountCode == null)
            {
                return HttpNotFound();
            }
            return View(discountCode);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            DiscountCode discountCode = db.DiscountCodes.Find(id);
			discountCode.IsDeleted=true;
			discountCode.DeletionDate=DateTime.Now;
 
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
