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
    public class SiteBlogImagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SiteBlogImages
        public ActionResult Index(Guid id)
        {
            var siteBlogImages = db.SiteBlogImages.Include(s => s.SiteBlog).Where(s=>s.SiteBlogId==id&& s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(siteBlogImages.ToList());
        }

       
        public ActionResult Create(Guid id)
        {
            ViewBag.SiteBlogId = id;
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteBlogImage siteBlogImage, HttpPostedFileBase fileupload, Guid id)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    siteBlogImage.ImageUrl = newFilenameUrl;
                }
                #endregion
                siteBlogImage.SiteBlogId= id;
                siteBlogImage.IsDeleted=false;
				siteBlogImage.CreationDate= DateTime.Now; 
                siteBlogImage.Id = Guid.NewGuid();
                db.SiteBlogImages.Add(siteBlogImage);
                db.SaveChanges();
                return RedirectToAction("Index",new {id=id});
            }

            ViewBag.SiteBlogId = id;
            return View(siteBlogImage);
        }

        // GET: SiteBlogImages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlogImage siteBlogImage = db.SiteBlogImages.Find(id);
            if (siteBlogImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteBlogId =  siteBlogImage.SiteBlogId ;
            return View(siteBlogImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SiteBlogImage siteBlogImage, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Blog/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    siteBlogImage.ImageUrl = newFilenameUrl;
                }
                #endregion

                siteBlogImage.LastModifiedDate = DateTime.Now;
                siteBlogImage.IsDeleted=false;
                db.Entry(siteBlogImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = siteBlogImage.SiteBlogId });

            }
            ViewBag.SiteBlogId = siteBlogImage.SiteBlogId;
            return View(siteBlogImage);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlogImage siteBlogImage = db.SiteBlogImages.Find(id);
            if (siteBlogImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteBlogId = siteBlogImage.SiteBlogId;

            return View(siteBlogImage);
        }

        // POST: SiteBlogImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SiteBlogImage siteBlogImage = db.SiteBlogImages.Find(id);
			siteBlogImage.IsDeleted=true;
			siteBlogImage.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = siteBlogImage.SiteBlogId });
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
