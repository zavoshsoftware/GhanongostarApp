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
    public class UserVipPackagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: UserVipPackages
        public ActionResult Index()
        {
            var userVipPackages = db.UserVipPackages.Include(u => u.User).Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.CreationDate);
            return View(userVipPackages.ToList());
        }

        // GET: UserVipPackages/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserVipPackage userVipPackage = db.UserVipPackages.Find(id);
            if (userVipPackage == null)
            {
                return HttpNotFound();
            }
            return View(userVipPackage);
        }

        // GET: UserVipPackages/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: UserVipPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,VipPackegeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserVipPackage userVipPackage)
        {
            if (ModelState.IsValid)
            {
				userVipPackage.IsDeleted=false;
				userVipPackage.CreationDate= DateTime.Now; 
                userVipPackage.Id = Guid.NewGuid();
                db.UserVipPackages.Add(userVipPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userVipPackage.UserId);
            return View(userVipPackage);
        }

        // GET: UserVipPackages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserVipPackage userVipPackage = db.UserVipPackages.Find(id);
            if (userVipPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userVipPackage.UserId);
            return View(userVipPackage);
        }

        // POST: UserVipPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,VipPackegeId,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] UserVipPackage userVipPackage)
        {
            if (ModelState.IsValid)
            {
				userVipPackage.IsDeleted=false;
                db.Entry(userVipPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", userVipPackage.UserId);
            return View(userVipPackage);
        }

        // GET: UserVipPackages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserVipPackage userVipPackage = db.UserVipPackages.Find(id);
            if (userVipPackage == null)
            {
                return HttpNotFound();
            }
            return View(userVipPackage);
        }

        // POST: UserVipPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UserVipPackage userVipPackage = db.UserVipPackages.Find(id);
			userVipPackage.IsDeleted=true;
			userVipPackage.DeletionDate=DateTime.Now;
 
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
