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
    public class PagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Pages
        public ActionResult Index()
        {
            return View(db.Pages.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }


        // GET: Pages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Page page)
        {
            if (ModelState.IsValid)
            {
				page.IsDeleted=false;
				page.CreationDate= DateTime.Now; 
                page.Id = Guid.NewGuid();
                db.Pages.Add(page);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(page);
        }

        // GET: Pages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Page page)
        {
            if (ModelState.IsValid)
            {
				page.IsDeleted=false;
                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Page page = db.Pages.Find(id);
			page.IsDeleted=true;
			page.DeletionDate=DateTime.Now;
 
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
