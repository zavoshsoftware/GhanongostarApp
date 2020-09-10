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
    public class RedirectsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Redirects
        public ActionResult Index()
        {
            return View(db.Redirects.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: Redirects/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Redirect redirect = db.Redirects.Find(id);
            if (redirect == null)
            {
                return HttpNotFound();
            }
            return View(redirect);
        }

        // GET: Redirects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Redirects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OldUrl,NewUrl,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Redirect redirect)
        {
            if (ModelState.IsValid)
            {
				redirect.IsDeleted=false;
				redirect.CreationDate= DateTime.Now; 
                redirect.Id = Guid.NewGuid();
                db.Redirects.Add(redirect);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(redirect);
        }

        // GET: Redirects/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Redirect redirect = db.Redirects.Find(id);
            if (redirect == null)
            {
                return HttpNotFound();
            }
            return View(redirect);
        }

        // POST: Redirects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OldUrl,NewUrl,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Redirect redirect)
        {
            if (ModelState.IsValid)
            {
				redirect.IsDeleted=false;
                db.Entry(redirect).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(redirect);
        }

        // GET: Redirects/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Redirect redirect = db.Redirects.Find(id);
            if (redirect == null)
            {
                return HttpNotFound();
            }
            return View(redirect);
        }

        // POST: Redirects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Redirect redirect = db.Redirects.Find(id);
			redirect.IsDeleted=true;
			redirect.DeletionDate=DateTime.Now;
 
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
