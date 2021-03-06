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
    public class ConsultantRequestFormsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ConsultantRequestForms
        public ActionResult Index()
        {
            return View(db.ConxConsultantRequestForms.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: ConsultantRequestForms/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequestForm consultantRequestForm = db.ConxConsultantRequestForms.Find(id);
            if (consultantRequestForm == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequestForm);
        }

        // GET: ConsultantRequestForms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConsultantRequestForms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Company,ActionType,EmployeeQuantity,ContactNumber,Message,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ConsultantRequestForm consultantRequestForm)
        {
            if (ModelState.IsValid)
            {
				consultantRequestForm.IsDeleted=false;
				consultantRequestForm.CreationDate= DateTime.Now; 
                consultantRequestForm.Id = Guid.NewGuid();
                db.ConxConsultantRequestForms.Add(consultantRequestForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(consultantRequestForm);
        }

        // GET: ConsultantRequestForms/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequestForm consultantRequestForm = db.ConxConsultantRequestForms.Find(id);
            if (consultantRequestForm == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequestForm);
        }

        // POST: ConsultantRequestForms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Company,ActionType,EmployeeQuantity,ContactNumber,Message,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] ConsultantRequestForm consultantRequestForm)
        {
            if (ModelState.IsValid)
            {
				consultantRequestForm.IsDeleted=false;
                db.Entry(consultantRequestForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consultantRequestForm);
        }

        // GET: ConsultantRequestForms/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantRequestForm consultantRequestForm = db.ConxConsultantRequestForms.Find(id);
            if (consultantRequestForm == null)
            {
                return HttpNotFound();
            }
            return View(consultantRequestForm);
        }

        // POST: ConsultantRequestForms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ConsultantRequestForm consultantRequestForm = db.ConxConsultantRequestForms.Find(id);
			consultantRequestForm.IsDeleted=true;
			consultantRequestForm.DeletionDate=DateTime.Now;
 
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
