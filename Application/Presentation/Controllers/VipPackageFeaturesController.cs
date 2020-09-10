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
    public class VipPackageFeaturesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: VipPackageFeatures
        public ActionResult Index(Guid id)
        {
            var vipPackageFeatures = db.VipPackageFeatures.Include(v => v.VipPackage).Where(v=>v.IsDeleted==false && v.VipPackageId==id).OrderByDescending(v=>v.CreationDate);
            ViewBag.title = db.VipPackages.Find(id).Title;
            ViewBag.id = id;
            return View(vipPackageFeatures.ToList());
        }

        // GET: VipPackageFeatures/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackageFeature vipPackageFeature = db.VipPackageFeatures.Find(id);
            if (vipPackageFeature == null)
            {
                return HttpNotFound();
            }
            return View(vipPackageFeature);
        }

        // GET: VipPackageFeatures/Create
        public ActionResult Create(Guid id)
        {
            ViewBag.VipPackageId = new SelectList(db.VipPackages.Where(current=>current.IsDeleted==false && current.IsActive==true), "Id", "Title");
            ViewBag.id = id;
            return View();
        }

        // POST: VipPackageFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VipPackageFeature vipPackageFeature,Guid id)
        {
            if (ModelState.IsValid)
            {
				vipPackageFeature.IsDeleted=false;
				vipPackageFeature.CreationDate= DateTime.Now; 
                vipPackageFeature.Id = Guid.NewGuid();
                vipPackageFeature.VipPackageId = id;
                db.VipPackageFeatures.Add(vipPackageFeature);
                db.SaveChanges();
                return RedirectToAction("Index",new { id=id});
            }
            ViewBag.id = vipPackageFeature.VipPackageId;
            ViewBag.VipPackageId = new SelectList(db.VipPackages.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", vipPackageFeature.VipPackageId);
            return View(vipPackageFeature);
        }

        // GET: VipPackageFeatures/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackageFeature vipPackageFeature = db.VipPackageFeatures.Find(id);
            if (vipPackageFeature == null)
            {
                return HttpNotFound();
            }
            ViewBag.VipPackageId = new SelectList(db.VipPackages.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", vipPackageFeature.VipPackageId);
            ViewBag.id = vipPackageFeature.VipPackageId;
            return View(vipPackageFeature);
        }

        // POST: VipPackageFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VipPackageFeature vipPackageFeature)
        {
            if (ModelState.IsValid)
            {
				vipPackageFeature.IsDeleted=false;
                db.Entry(vipPackageFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { id=vipPackageFeature.VipPackageId});
            }
            ViewBag.VipPackageId = new SelectList(db.VipPackages.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", vipPackageFeature.VipPackageId);
            ViewBag.id = vipPackageFeature.VipPackageId;
            return View(vipPackageFeature);
        }

        // GET: VipPackageFeatures/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackageFeature vipPackageFeature = db.VipPackageFeatures.Find(id);
            if (vipPackageFeature == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = vipPackageFeature.VipPackageId;
            return View(vipPackageFeature);
        }

        // POST: VipPackageFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VipPackageFeature vipPackageFeature = db.VipPackageFeatures.Find(id);
			vipPackageFeature.IsDeleted=true;
			vipPackageFeature.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            ViewBag.id = vipPackageFeature.VipPackageId;
            return RedirectToAction("Index",new { id=vipPackageFeature.VipPackageId});
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
