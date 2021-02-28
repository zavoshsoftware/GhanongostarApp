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
    public class EmpClubProductGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EmpClubProductGroups
        public ActionResult Index()
        {
            return View(db.EmpClubProductGroups.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: EmpClubProductGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProductGroup empClubProductGroup = db.EmpClubProductGroups.Find(id);
            if (empClubProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubProductGroup);
        }

        // GET: EmpClubProductGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpClubProductGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EmpClubProductGroup empClubProductGroup)
        {
            if (ModelState.IsValid)
            {
				empClubProductGroup.IsDeleted=false;
				empClubProductGroup.CreationDate= DateTime.Now; 
                empClubProductGroup.Id = Guid.NewGuid();
                db.EmpClubProductGroups.Add(empClubProductGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empClubProductGroup);
        }

        // GET: EmpClubProductGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProductGroup empClubProductGroup = db.EmpClubProductGroups.Find(id);
            if (empClubProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubProductGroup);
        }

        // POST: EmpClubProductGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EmpClubProductGroup empClubProductGroup)
        {
            if (ModelState.IsValid)
            {
				empClubProductGroup.IsDeleted=false;
                db.Entry(empClubProductGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empClubProductGroup);
        }

        // GET: EmpClubProductGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProductGroup empClubProductGroup = db.EmpClubProductGroups.Find(id);
            if (empClubProductGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubProductGroup);
        }

        // POST: EmpClubProductGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmpClubProductGroup empClubProductGroup = db.EmpClubProductGroups.Find(id);
			empClubProductGroup.IsDeleted=true;
			empClubProductGroup.DeletionDate=DateTime.Now;
 
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
