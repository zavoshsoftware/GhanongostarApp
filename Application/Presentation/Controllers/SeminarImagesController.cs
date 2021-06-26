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
    public class SeminarImagesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: SeminarImages
        public ActionResult Index(Guid id)
        {
            var seminarImages = db.SeminarImages.Include(s => s.Seminar).Where(s=>s.SeminarId==id&& s.IsDeleted==false).OrderByDescending(s=>s.CreationDate);
            return View(seminarImages.ToList());
        }
         
        public ActionResult Create(Guid id)
        {
            ViewBag.SeminarId = id;
            return View();
        }

        // POST: SeminarImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( SeminarImage seminarImage,Guid id, HttpPostedFileBase fileupload)
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
                    seminarImage.ImageUrl = newFilenameUrl;
                }
                #endregion
                seminarImage.IsDeleted=false;
				seminarImage.CreationDate= DateTime.Now; 
                seminarImage.Id = Guid.NewGuid();
                db.SeminarImages.Add(seminarImage);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.SeminarId = id;
            return View(seminarImage);
        }

        // GET: SeminarImages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarImage seminarImage = db.SeminarImages.Find(id);
            if (seminarImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeminarId =   seminarImage.SeminarId;
            return View(seminarImage);
        }

        // POST: SeminarImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SeminarImage seminarImage,  HttpPostedFileBase fileupload)
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
                    seminarImage.ImageUrl = newFilenameUrl;
                }
                #endregion
                seminarImage.IsDeleted=false;
                db.Entry(seminarImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=seminarImage.SeminarId});
            }
            ViewBag.SeminarId =  seminarImage.SeminarId;
            return View(seminarImage);
        }

        // GET: SeminarImages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeminarImage seminarImage = db.SeminarImages.Find(id);
            if (seminarImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.SeminarId = seminarImage.SeminarId;
            return View(seminarImage);
        }

        // POST: SeminarImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            SeminarImage seminarImage = db.SeminarImages.Find(id);
			seminarImage.IsDeleted=true;
			seminarImage.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = seminarImage.SeminarId });
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
