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
    public class ConsultantRequestsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ConsultantRequests
        public ActionResult Index()
        {
            return View(db.ConsultantRequests.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ConsultantRequests/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequest consultantRequest = db.ConsultantRequests.Find(id);
            if (consultantRequest == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequest);
        }

        // GET: ConsultantRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConsultantRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Company,ContactNumber,Message,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ConsultantRequest consultantRequest)
        {
            if (ModelState.IsValid)
            {
				consultantRequest.IsDeleted=false;
				consultantRequest.CreationDate= DateTime.Now; 
                consultantRequest.Id = Guid.NewGuid();
                db.ConsultantRequests.Add(consultantRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consultantRequest);
        }

        // GET: ConsultantRequests/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequest consultantRequest = db.ConsultantRequests.Find(id);
            if (consultantRequest == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequest);
        }

        // POST: ConsultantRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Company,ContactNumber,Message,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ConsultantRequest consultantRequest)
        {
            if (ModelState.IsValid)
            {
				consultantRequest.IsDeleted=false;
                db.Entry(consultantRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consultantRequest);
        }

        // GET: ConsultantRequests/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequest consultantRequest = db.ConsultantRequests.Find(id);
            if (consultantRequest == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequest);
        }

        // POST: ConsultantRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ConsultantRequest consultantRequest = db.ConsultantRequests.Find(id);
			consultantRequest.IsDeleted=true;
			consultantRequest.DeletionDate=DateTime.Now;
 
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
