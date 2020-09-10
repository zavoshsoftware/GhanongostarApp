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
    public class ProductsController : Controller
    {
        
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        public ActionResult Index(Guid id)
        {
            var products = db.Products.Include(p => p.ProductType).Where(p => p.IsDeleted == false && p.ProductTypeId == id).OrderByDescending(p => p.CreationDate);

            if (db.ProductTypes.Find(id).Name.ToLower() == "course")
                ViewBag.isOnlineCourse = "true";
            return View(products.ToList());
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.ProductTypeId = id;
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current=>current.IsDeleted==false).ToList(), "Id", "Title");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product product, Guid id, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2, HttpPostedFileBase videoThumbnail)
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
                    product.ImageUrl = newFilenameUrl;
                }
                #endregion

                #region Upload and resize image if needed
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Product/Files/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload2.SaveAs(physicalFilename);
                    product.FileUrl = newFilenameUrl;
                }
                #endregion
                #region Upload and resize video thumbnail if needed
                if (videoThumbnail != null)
                {
                    string thumbname = Path.GetFileName(videoThumbnail.FileName);
                    string newThumbname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(thumbname);

                    string newThumbnameUrl = "/Uploads/Product/" + newThumbname;
                    string physicalThumbname = Server.MapPath(newThumbnameUrl);
                    videoThumbnail.SaveAs(physicalThumbname);
                    product.VideoThumbnail = newThumbnameUrl;
                }
                #endregion
                product.Code = FindeLastOrderCode() + 1;
                product.ProductTypeId = id;
                product.IsDeleted = false;
                product.CreationDate = DateTime.Now;
                product.Id = Guid.NewGuid();
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current => current.IsDeleted == false).ToList(), "Id", "Title");
            ViewBag.ProductTypeId = id;
            return View(product);
        }

        public int FindeLastOrderCode()
        {
            Product product =db.Products.Where(current=>current.IsDeleted==false).OrderByDescending(current => current.Code).FirstOrDefault();
            if (product != null)
                return product.Code;
            else
                return 999;
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeId = product.ProductTypeId;
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current => current.IsDeleted == false).ToList(), "Id", "Title",product.ProductGroupId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Product product, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2,HttpPostedFileBase videoThumbnail)
        {
            if (ModelState.IsValid)
            {
                string newThumbnameUrl = product.VideoThumbnail;
                string newFilenameUrl1 = product.ImageUrl;
                string newFilenameUrl2 = product.FileUrl;
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl1 = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl1);
                    fileupload.SaveAs(physicalFilename);
                    product.ImageUrl = newFilenameUrl1;
                }
                #endregion

                #region Upload and resize image if needed
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                     newFilenameUrl2 = "/Uploads/Product/Files/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl2);
                    fileupload2.SaveAs(physicalFilename);
                    product.FileUrl = newFilenameUrl2;
                }
                #endregion
                #region Upload and resize video thumbnail if needed
                if (videoThumbnail != null)
                {
                    
                    string thumbname = Path.GetFileName(videoThumbnail.FileName);
                    string newThumbname = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(thumbname);

                     newThumbnameUrl = "/Uploads/Product/" + newThumbname;
                    string physicalThumbname = Server.MapPath(newThumbnameUrl);
                    videoThumbnail.SaveAs(physicalThumbname);
                    product.VideoThumbnail = newThumbnameUrl;
                }
                #endregion
                product.IsDeleted = false;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {id = product.ProductTypeId});
            }
            ViewBag.ProductTypeId = product.ProductTypeId;
            ViewBag.ProductGroupId = new SelectList(db.ProductGroups.Where(current => current.IsDeleted == false).ToList(), "Id", "Title",product.ProductGroupId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = db.Products.Find(id);
            product.IsDeleted = true;
            product.DeletionDate = DateTime.Now;

            db.SaveChanges();
            ViewBag.ProductTypeId = product.ProductTypeId;
            return RedirectToAction("Index",new{id=product.ProductTypeId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [AllowAnonymous]
        [Route("product/detail/{id:Guid}")]
        public ActionResult DetailWebView(Guid id)
        {
            Product product = db.Products.Find(id);

            if (product == null)
                return HttpNotFound();


            return View(product);
        }
    }
}
