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
    public class VipPackagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: VipPackages
        public ActionResult Index()
        {
            return View(db.VipPackages.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: VipPackages/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackage vipPackage = db.VipPackages.Find(id);
            if (vipPackage == null)
            {
                return HttpNotFound();
            }
            return View(vipPackage);
        }

        // GET: VipPackages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VipPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VipPackage vipPackage)
        {
            if (ModelState.IsValid)
            {
               
                vipPackage.IsDeleted=false;
				vipPackage.CreationDate= DateTime.Now; 
                vipPackage.Id = Guid.NewGuid();
                vipPackage.ProductId = InsertProduct(vipPackage.Title);
                db.VipPackages.Add(vipPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vipPackage);
        }

        // GET: VipPackages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackage vipPackage = db.VipPackages.Find(id);
            if (vipPackage == null)
            {
                return HttpNotFound();
            }
            return View(vipPackage);
        }

        // POST: VipPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VipPackage vipPackage)
        {
            if (ModelState.IsValid)
            {
				vipPackage.IsDeleted=false;
                db.Entry(vipPackage).State = EntityState.Modified;
                EditProduct(vipPackage.ProductId, vipPackage.Title,vipPackage.Price);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vipPackage);
        }

        // GET: VipPackages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VipPackage vipPackage = db.VipPackages.Find(id);
            if (vipPackage == null)
            {
                return HttpNotFound();
            }
            return View(vipPackage);
        }

        // POST: VipPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VipPackage vipPackage = db.VipPackages.Find(id);
			vipPackage.IsDeleted=true;
			vipPackage.DeletionDate=DateTime.Now;
 
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

        public Guid InsertProduct(string title)
        {
            Product product = new Product();
            product.Id = Guid.NewGuid();
            product.IsVip = false;
            product.IsFree = false;
            product.IsActive = true;
            product.IsDeleted = false;
            product.Title = title;
            product.Code= FindeLastOrderCode() + 1;
            product.ProductTypeId = new Guid("44834033-93d1-4c9b-a130-24457f0f7057");
            product.CreationDate = DateTime.Now;

            db.Products.Add(product);
            db.SaveChanges();

            return product.Id;
        }

        public void EditProduct(Guid id,string title,decimal price)
        {
            Product product = db.Products.Find(id);
            if (product != null)
            {
                product.Title = title;
                product.Amount = price;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
                

        }
        public int FindeLastOrderCode()
        {
            Product product = db.Products.Where(current => current.IsDeleted == false).OrderByDescending(current => current.Code).FirstOrDefault();
            if (product != null)
                return product.Code;
            else
                return 999;
        }
    }
}
