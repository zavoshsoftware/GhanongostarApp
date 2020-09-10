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
    public class ProductDiscountsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index(Guid id)
        {
            DiscountCode discountCode = db.DiscountCodes.Find(id);

            ViewBag.Title = "فهرست محصولات مرتبط با کد تخفیف " + discountCode.Code;
            var productDiscounts = db.ProductDiscounts.Include(p => p.DiscountCode)
                .Where(p => p.IsDeleted == false && p.DiscountCodeId == id).OrderByDescending(p => p.CreationDate)
                .Include(p => p.Product);

            return View(productDiscounts.ToList());
        }

       
        public ActionResult Create(Guid id)
        {
            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code",id);
            ViewBag.ProductId = new SelectList(db.Products.Where(current=>current.IsDeleted==false&&current.IsFree==false), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDiscount productDiscount,Guid id)
        {
            if (ModelState.IsValid)
            {
                productDiscount.DiscountCodeId = id;
                productDiscount.IsDeleted = false;
                productDiscount.CreationDate = DateTime.Now;
                productDiscount.Id = Guid.NewGuid();
                db.ProductDiscounts.Add(productDiscount);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.DiscountCodeId = new SelectList(db.DiscountCodes, "Id", "Code", id);
            ViewBag.ProductId = new SelectList(db.Products.Where(current => current.IsDeleted == false && current.IsFree == false), "Id", "Title");
            return View(productDiscount);
        }
 
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductDiscount productDiscount = db.ProductDiscounts.Find(id);
            if (productDiscount == null)
            {
                return HttpNotFound();
            }
            return View(productDiscount);
        }

        // POST: ProductDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductDiscount productDiscount = db.ProductDiscounts.Find(id);
            productDiscount.IsDeleted = true;
            productDiscount.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index",new{id=productDiscount.DiscountCodeId});
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
