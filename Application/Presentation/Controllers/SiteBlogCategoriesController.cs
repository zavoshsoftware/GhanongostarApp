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
    public class SiteBlogCategoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SiteBlogCategories
        public ActionResult Index()
        {
            return View(db.SiteBlogCategories.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: SiteBlogCategories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlogCategory siteBlogCategory = db.SiteBlogCategories.Find(id);
            if (siteBlogCategory == null)
            {
                return HttpNotFound();
            }
            return View(siteBlogCategory);
        }

        // GET: SiteBlogCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteBlogCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteBlogCategory siteBlogCategory)
        {
            if (ModelState.IsValid)
            {
				siteBlogCategory.IsDeleted=false;
				siteBlogCategory.CreationDate= DateTime.Now; 
                siteBlogCategory.Id = Guid.NewGuid();
                db.SiteBlogCategories.Add(siteBlogCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteBlogCategory);
        }

        // GET: SiteBlogCategories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlogCategory siteBlogCategory = db.SiteBlogCategories.Find(id);
            if (siteBlogCategory == null)
            {
                return HttpNotFound();
            }
            return View(siteBlogCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SiteBlogCategory siteBlogCategory)
        {
            if (ModelState.IsValid)
            {
				siteBlogCategory.LastModifiedDate=DateTime.Now;
				siteBlogCategory.IsDeleted=false;
                db.Entry(siteBlogCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(siteBlogCategory);
        }

        // GET: SiteBlogCategories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlogCategory siteBlogCategory = db.SiteBlogCategories.Find(id);
            if (siteBlogCategory == null)
            {
                return HttpNotFound();
            }
            return View(siteBlogCategory);
        }

        // POST: SiteBlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SiteBlogCategory siteBlogCategory = db.SiteBlogCategories.Find(id);
			siteBlogCategory.IsDeleted=true;
			siteBlogCategory.DeletionDate=DateTime.Now;
 
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
