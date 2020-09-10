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
    public class VersionHistoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: VersionHistories
        public ActionResult Index()
        {
            return View(db.VersionHistories.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: VersionHistories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionHistory versionHistory = db.VersionHistories.Find(id);
            if (versionHistory == null)
            {
                return HttpNotFound();
            }
            return View(versionHistory);
        }

        // GET: VersionHistories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VersionHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VersionNumber,Os,IsNeccessary,LatestStableVersion,IsBeta,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] VersionHistory versionHistory)
        {
            if (ModelState.IsValid)
            {
				versionHistory.IsDeleted=false;
				versionHistory.CreationDate= DateTime.Now; 
                versionHistory.Id = Guid.NewGuid();
                db.VersionHistories.Add(versionHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(versionHistory);
        }

        // GET: VersionHistories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionHistory versionHistory = db.VersionHistories.Find(id);
            if (versionHistory == null)
            {
                return HttpNotFound();
            }
            return View(versionHistory);
        }

        // POST: VersionHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VersionNumber,Os,IsNeccessary,LatestStableVersion,IsBeta,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] VersionHistory versionHistory)
        {
            if (ModelState.IsValid)
            {
				versionHistory.IsDeleted=false;
                db.Entry(versionHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(versionHistory);
        }

        // GET: VersionHistories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VersionHistory versionHistory = db.VersionHistories.Find(id);
            if (versionHistory == null)
            {
                return HttpNotFound();
            }
            return View(versionHistory);
        }

        // POST: VersionHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VersionHistory versionHistory = db.VersionHistories.Find(id);
			versionHistory.IsDeleted=true;
			versionHistory.DeletionDate=DateTime.Now;
 
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
