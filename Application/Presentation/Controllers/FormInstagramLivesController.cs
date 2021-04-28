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
    public class FormInstagramLivesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FormInstagramLives
        public ActionResult Index()
        {
            return View(db.FormInstagramLives.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: FormInstagramLives/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormInstagramLive formInstagramLive = db.FormInstagramLives.Find(id);
            if (formInstagramLive == null)
            {
                return HttpNotFound();
            }
            return View(formInstagramLive);
        }

        // GET: FormInstagramLives/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormInstagramLives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,InstagramId,ContactNumber,IsPaid,OrderCode,SaleRefrenceId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] FormInstagramLive formInstagramLive)
        {
            if (ModelState.IsValid)
            {
				formInstagramLive.IsDeleted=false;
				formInstagramLive.CreationDate= DateTime.Now; 
                formInstagramLive.Id = Guid.NewGuid();
                db.FormInstagramLives.Add(formInstagramLive);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formInstagramLive);
        }

        // GET: FormInstagramLives/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormInstagramLive formInstagramLive = db.FormInstagramLives.Find(id);
            if (formInstagramLive == null)
            {
                return HttpNotFound();
            }
            return View(formInstagramLive);
        }

        // POST: FormInstagramLives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,InstagramId,ContactNumber,IsPaid,OrderCode,SaleRefrenceId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] FormInstagramLive formInstagramLive)
        {
            if (ModelState.IsValid)
            {
				formInstagramLive.IsDeleted=false;
                db.Entry(formInstagramLive).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formInstagramLive);
        }

        // GET: FormInstagramLives/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormInstagramLive formInstagramLive = db.FormInstagramLives.Find(id);
            if (formInstagramLive == null)
            {
                return HttpNotFound();
            }
            return View(formInstagramLive);
        }

        // POST: FormInstagramLives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FormInstagramLive formInstagramLive = db.FormInstagramLives.Find(id);
			formInstagramLive.IsDeleted=true;
			formInstagramLive.DeletionDate=DateTime.Now;
 
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
