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
    public class SiteBlogsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SiteBlogs
        public ActionResult Index()
        {
            var siteBlogs = db.SiteBlogs.Include(s => s.SiteBlogCategory).Where(s=>s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(siteBlogs.ToList());
        }

        // GET: SiteBlogs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlog siteBlog = db.SiteBlogs.Find(id);
            if (siteBlog == null)
            {
                return HttpNotFound();
            }
            return View(siteBlog);
        }

        // GET: SiteBlogs/Create
        public ActionResult Create()
        {
            ViewBag.SiteBlogCategoryId = new SelectList(db.SiteBlogCategories, "Id", "Title");
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create( SiteBlog siteBlog, HttpPostedFileBase fileupload)
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
                    siteBlog.ImageUrl = newFilenameUrl;
                }
                #endregion
                siteBlog.IsDeleted=false;
				siteBlog.CreationDate= DateTime.Now; 
                siteBlog.Id = Guid.NewGuid();
                db.SiteBlogs.Add(siteBlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SiteBlogCategoryId = new SelectList(db.SiteBlogCategories, "Id", "Title", siteBlog.SiteBlogCategoryId);
            return View(siteBlog);
        }

        // GET: SiteBlogs/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlog siteBlog = db.SiteBlogs.Find(id);
            if (siteBlog == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteBlogCategoryId = new SelectList(db.SiteBlogCategories, "Id", "Title", siteBlog.SiteBlogCategoryId);
            return View(siteBlog);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(SiteBlog siteBlog, HttpPostedFileBase fileupload)
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
                    siteBlog.ImageUrl = newFilenameUrl;
                }
                #endregion
                siteBlog.IsDeleted=false;
                siteBlog.LastModifiedDate = DateTime.Now;
                db.Entry(siteBlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteBlogCategoryId = new SelectList(db.SiteBlogCategories, "Id", "Title", siteBlog.SiteBlogCategoryId);
            return View(siteBlog);
        }

        // GET: SiteBlogs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteBlog siteBlog = db.SiteBlogs.Find(id);
            if (siteBlog == null)
            {
                return HttpNotFound();
            }
            return View(siteBlog);
        }

        // POST: SiteBlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SiteBlog siteBlog = db.SiteBlogs.Find(id);
			siteBlog.IsDeleted=true;
			siteBlog.DeletionDate=DateTime.Now;
 
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
