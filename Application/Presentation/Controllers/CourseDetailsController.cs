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
    public class CourseDetailsController : Controller
    {
        
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: CourseDetails
        public ActionResult Index(Guid id)
        {
            var courseDetails = db.CourseDetails.Include(c => c.Product).Where(c => c.IsDeleted == false && c.ProductId == id).OrderByDescending(c => c.CreationDate);
            ViewBag.Title = db.Products.Find(id)?.Title;
            return View(courseDetails.ToList());
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.ProductId = id;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseDetail courseDetail, Guid id, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Courses/Video/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    courseDetail.VideoUrl = newFilenameUrl;
                }
                #endregion

                #region Upload and resize image if needed
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Courses/Images/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload2.SaveAs(physicalFilename);
                    courseDetail.ThumbnailImageUrl = newFilenameUrl;
                }
                #endregion
                courseDetail.ProductId = id;
                courseDetail.IsDeleted = false;
                courseDetail.CreationDate = DateTime.Now;
                courseDetail.Id = Guid.NewGuid();
                db.CourseDetails.Add(courseDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProductId = id;
            return View(courseDetail);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = id;
            return View(courseDetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CourseDetail courseDetail, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Courses/Video/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    courseDetail.VideoUrl = newFilenameUrl;
                }
                #endregion

                #region Upload and resize image if needed
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/Courses/Images/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload2.SaveAs(physicalFilename);
                    courseDetail.ThumbnailImageUrl = newFilenameUrl;
                }
                #endregion

                courseDetail.IsDeleted = false;
                db.Entry(courseDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = courseDetail.ProductId });
            }
            ViewBag.ProductId = courseDetail.Id;
            return View(courseDetail);
        }

        // GET: CourseDetails/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            if (courseDetail == null)
            {
                return HttpNotFound();
            }
            return View(courseDetail);
        }

        // POST: CourseDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CourseDetail courseDetail = db.CourseDetails.Find(id);
            courseDetail.IsDeleted = true;
            courseDetail.DeletionDate = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("Index", new { id = id });
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
