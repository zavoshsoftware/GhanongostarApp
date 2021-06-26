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
    public class SeminarTeachersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SeminarTeachers
        public ActionResult Index(Guid id)
        {
            var seminarTeachers = db.SeminarTeachers.Include(s => s.Seminar)
                .Where(s => s.SeminarId == id && s.IsDeleted == false).OrderByDescending(s => s.CreationDate);

            return View(seminarTeachers.ToList());
        }
         
        public ActionResult Create(Guid id)
        {
            ViewBag.SeminarId = id;
            return View();
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SeminarTeacher seminarTeacher , Guid id, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seminar/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    seminarTeacher.ImageUrl = newFilenameUrl;
                }
                #endregion
                seminarTeacher.SeminarId=id;
                seminarTeacher.IsDeleted=false;
				seminarTeacher.CreationDate= DateTime.Now; 
                seminarTeacher.Id = Guid.NewGuid();
                db.SeminarTeachers.Add(seminarTeacher);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.SeminarId =  seminarTeacher.SeminarId;
            return View(seminarTeacher);
        }

        // GET: SeminarTeachers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarTeacher seminarTeacher = db.SeminarTeachers.Find(id);
            if (seminarTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeminarId =  seminarTeacher.SeminarId ;
            return View(seminarTeacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SeminarTeacher seminarTeacher, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/seminar/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    seminarTeacher.ImageUrl = newFilenameUrl;
                }
                #endregion
 
                seminarTeacher.IsDeleted=false;
                db.Entry(seminarTeacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=seminarTeacher.SeminarId});
            }
            ViewBag.SeminarId = seminarTeacher.SeminarId;
            return View(seminarTeacher);
        }

        // GET: SeminarTeachers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarTeacher seminarTeacher = db.SeminarTeachers.Find(id);
            if (seminarTeacher == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeminarId = seminarTeacher.SeminarId;
            return View(seminarTeacher);
        }

        // POST: SeminarTeachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SeminarTeacher seminarTeacher = db.SeminarTeachers.Find(id);
			seminarTeacher.IsDeleted=true;
			seminarTeacher.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = seminarTeacher.SeminarId });
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
