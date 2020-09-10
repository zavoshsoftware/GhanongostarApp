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
    public class TextsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Texts
        public ActionResult Index()
        {
            return View(db.Texts.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: Texts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            return View(text);
        }

        // GET: Texts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Texts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Body,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Text text)
        {
            if (ModelState.IsValid)
            {
				text.IsDeleted=false;
				text.CreationDate= DateTime.Now; 
                text.Id = Guid.NewGuid();
                db.Texts.Add(text);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(text);
        }

        // GET: Texts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            return View(text);
        }

        // POST: Texts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Text text)
        {
            if (ModelState.IsValid)
            {
				text.IsDeleted=false;
                db.Entry(text).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(text);
        }

        // GET: Texts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Text text = db.Texts.Find(id);
            if (text == null)
            {
                return HttpNotFound();
            }
            return View(text);
        }

        // POST: Texts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Text text = db.Texts.Find(id);
			text.IsDeleted=true;
			text.DeletionDate=DateTime.Now;
 
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
