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
    public class EmpClubVideoGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EmpClubVideoGroups
        public ActionResult Index()
        {
            return View(db.EmpClubVideoGroups.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: EmpClubVideoGroups/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubVideoGroup empClubVideoGroup = db.EmpClubVideoGroups.Find(id);
            if (empClubVideoGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubVideoGroup);
        }

        // GET: EmpClubVideoGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpClubVideoGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EmpClubVideoGroup empClubVideoGroup)
        {
            if (ModelState.IsValid)
            {
				empClubVideoGroup.IsDeleted=false;
				empClubVideoGroup.CreationDate= DateTime.Now; 
                empClubVideoGroup.Id = Guid.NewGuid();
                db.EmpClubVideoGroups.Add(empClubVideoGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empClubVideoGroup);
        }

        // GET: EmpClubVideoGroups/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubVideoGroup empClubVideoGroup = db.EmpClubVideoGroups.Find(id);
            if (empClubVideoGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubVideoGroup);
        }

        // POST: EmpClubVideoGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EmpClubVideoGroup empClubVideoGroup)
        {
            if (ModelState.IsValid)
            {
				empClubVideoGroup.IsDeleted=false;
                db.Entry(empClubVideoGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empClubVideoGroup);
        }

        // GET: EmpClubVideoGroups/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubVideoGroup empClubVideoGroup = db.EmpClubVideoGroups.Find(id);
            if (empClubVideoGroup == null)
            {
                return HttpNotFound();
            }
            return View(empClubVideoGroup);
        }

        // POST: EmpClubVideoGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmpClubVideoGroup empClubVideoGroup = db.EmpClubVideoGroups.Find(id);
			empClubVideoGroup.IsDeleted=true;
			empClubVideoGroup.DeletionDate=DateTime.Now;
 
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
