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
    public class BlogsController : Controller
    {
        
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: Blogs
        public ActionResult Index()
        {
            var blogs = db.Blogs.Include(b => b.BlogCategory).Where(b=>b.IsDeleted==false).OrderByDescending(b=>b.CreationDate);
            return View(blogs.ToList());
        }

        // GET: Blogs/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // GET: Blogs/Create
        public ActionResult Create()
        {
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title");
            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Blog blog, HttpPostedFileBase fileupload)
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
                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted=false;
				blog.CreationDate= DateTime.Now; 
                blog.Id = Guid.NewGuid();
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(Blog blog, HttpPostedFileBase fileupload)
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
                    blog.ImageUrl = newFilenameUrl;
                }
                #endregion
                blog.IsDeleted=false;
                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogCategoryId = new SelectList(db.BlogCategories, "Id", "Title", blog.BlogCategoryId);
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Blog blog = db.Blogs.Find(id);
			blog.IsDeleted=true;
			blog.DeletionDate=DateTime.Now;
 
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

        [AllowAnonymous]
        [Route("blog/detail/{id:Guid}")]
        public ActionResult DetailWebView(Guid id)
        {
            Blog blog = db.Blogs.Find(id);

            if (blog == null)
                return HttpNotFound();


            return View(blog);
        }
    }
}
