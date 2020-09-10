using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    [Authorize(Roles = "SuperAdministrator")]
    public class ProductGroupsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ActionResult Index()
        {
            return View(db.ProductGroups.Where(a => a.IsDeleted == false).OrderByDescending(a => a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroup productGroup, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    productGroup.ImageUrl = newFilenameUrl;
                }
                #endregion

                productGroup.Code = ReturnCode();
                productGroup.IsDeleted = false;
                productGroup.CreationDate = DateTime.Now;
                productGroup.Id = Guid.NewGuid();
                db.ProductGroups.Add(productGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productGroup);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroup productGroup, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    productGroup.ImageUrl = newFilenameUrl;
                }
                #endregion
                productGroup.IsDeleted = false;
                db.Entry(productGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productGroup);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = db.ProductGroups.Find(id);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductGroup productGroup = db.ProductGroups.Find(id);
            productGroup.IsDeleted = true;
            productGroup.DeletionDate = DateTime.Now;

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


        public int ReturnCode()
        {
            ProductGroup productGroup = db.ProductGroups.Where(current => current.IsDeleted == false).OrderByDescending(current => current.Code).FirstOrDefault();
            if (productGroup != null)
            {
                return productGroup.Code + 1;
            }
            return 100;
        }
    }
}
