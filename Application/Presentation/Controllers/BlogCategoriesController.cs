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
    public class BlogCategoriesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]

        // GET: BlogCategories
        public ActionResult Index()
        {
            return View(db.BlogCategories.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

      
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] BlogCategory blogCategory)
        {
            if (ModelState.IsValid)
            {
				blogCategory.IsDeleted=false;
				blogCategory.CreationDate= DateTime.Now; 
                blogCategory.Id = Guid.NewGuid();
                db.BlogCategories.Add(blogCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogCategory);
        }

        // GET: BlogCategories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategory blogCategory = db.BlogCategories.Find(id);
            if (blogCategory == null)
            {
                return HttpNotFound();
            }
            return View(blogCategory);
        }

        // POST: BlogCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Order,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] BlogCategory blogCategory)
        {
            if (ModelState.IsValid)
            {
				blogCategory.IsDeleted=false;
                db.Entry(blogCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogCategory);
        }

        // GET: BlogCategories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogCategory blogCategory = db.BlogCategories.Find(id);
            if (blogCategory == null)
            {
                return HttpNotFound();
            }
            return View(blogCategory);
        }

        // POST: BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            BlogCategory blogCategory = db.BlogCategories.Find(id);
			blogCategory.IsDeleted=true;
			blogCategory.DeletionDate=DateTime.Now;
 
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
