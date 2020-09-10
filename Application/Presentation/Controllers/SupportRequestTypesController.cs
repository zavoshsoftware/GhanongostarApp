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
    public class SupportRequestTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: SupportRequestTypes
        public ActionResult Index()
        {
            return View(db.SupportRequestTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: SupportRequestTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequestType supportRequestType = db.SupportRequestTypes.Find(id);
            if (supportRequestType == null)
            {
                return HttpNotFound();
            }
            return View(supportRequestType);
        }

        // GET: SupportRequestTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupportRequestTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SupportRequestType supportRequestType)
        {
            if (ModelState.IsValid)
            {
				supportRequestType.IsDeleted=false;
				supportRequestType.CreationDate= DateTime.Now; 
                supportRequestType.Id = Guid.NewGuid();
                db.SupportRequestTypes.Add(supportRequestType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supportRequestType);
        }

        // GET: SupportRequestTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequestType supportRequestType = db.SupportRequestTypes.Find(id);
            if (supportRequestType == null)
            {
                return HttpNotFound();
            }
            return View(supportRequestType);
        }

        // POST: SupportRequestTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SupportRequestType supportRequestType)
        {
            if (ModelState.IsValid)
            {
				supportRequestType.IsDeleted=false;
                db.Entry(supportRequestType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supportRequestType);
        }

        // GET: SupportRequestTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequestType supportRequestType = db.SupportRequestTypes.Find(id);
            if (supportRequestType == null)
            {
                return HttpNotFound();
            }
            return View(supportRequestType);
        }

        // POST: SupportRequestTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SupportRequestType supportRequestType = db.SupportRequestTypes.Find(id);
			supportRequestType.IsDeleted=true;
			supportRequestType.DeletionDate=DateTime.Now;
 
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
