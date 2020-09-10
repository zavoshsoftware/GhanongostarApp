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
    public class SupportRequestsController : Controller
    {
        
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: SupportRequests
        public ActionResult Index()
        {
            var supportRequests = db.SupportRequests.Include(s => s.Type).Where(s=>s.IsDeleted==false).OrderByDescending(s=>s.CreationDate).Include(s => s.User).Where(s=>s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(supportRequests.ToList());
        }

        // GET: SupportRequests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequest supportRequest = db.SupportRequests.Find(id);
            if (supportRequest == null)
            {
                return HttpNotFound();
            }
            return View(supportRequest);
        }

        // GET: SupportRequests/Create
        public ActionResult Create()
        {
            ViewBag.SupportRequestTypeId = new SelectList(db.SupportRequestTypes, "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: SupportRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SupportRequestTypeId,Body,Status,Response,Code,UserId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SupportRequest supportRequest)
        {
            if (ModelState.IsValid)
            {
				supportRequest.IsDeleted=false;
				supportRequest.CreationDate= DateTime.Now; 
                supportRequest.Id = Guid.NewGuid();
                db.SupportRequests.Add(supportRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupportRequestTypeId = new SelectList(db.SupportRequestTypes, "Id", "Title", supportRequest.SupportRequestTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", supportRequest.UserId);
            return View(supportRequest);
        }

        // GET: SupportRequests/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequest supportRequest = db.SupportRequests.Find(id);
            if (supportRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupportRequestTypeId = new SelectList(db.SupportRequestTypes, "Id", "Title", supportRequest.SupportRequestTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", supportRequest.UserId);
            return View(supportRequest);
        }

        // POST: SupportRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SupportRequestTypeId,Body,Status,Response,Code,UserId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] SupportRequest supportRequest)
        {
            if (ModelState.IsValid)
            {
				supportRequest.IsDeleted=false;
                db.Entry(supportRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupportRequestTypeId = new SelectList(db.SupportRequestTypes, "Id", "Title", supportRequest.SupportRequestTypeId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", supportRequest.UserId);
            return View(supportRequest);
        }

        // GET: SupportRequests/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupportRequest supportRequest = db.SupportRequests.Find(id);
            if (supportRequest == null)
            {
                return HttpNotFound();
            }
            return View(supportRequest);
        }

        // POST: SupportRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SupportRequest supportRequest = db.SupportRequests.Find(id);
			supportRequest.IsDeleted=true;
			supportRequest.DeletionDate=DateTime.Now;
 
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
