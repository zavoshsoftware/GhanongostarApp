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
    public class EmpClubProductsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EmpClubProducts
        public ActionResult Index()
        {
            return View(db.EmpClubProducts.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: EmpClubProducts/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProduct empClubProduct = db.EmpClubProducts.Find(id);
            if (empClubProduct == null)
            {
                return HttpNotFound();
            }
            return View(empClubProduct);
        }

        // GET: EmpClubProducts/Create
        public ActionResult Create()
        {
            ViewBag.EmpClubProductGroupId = new SelectList(db.EmpClubProductGroups, "Id", "Title");
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( EmpClubProduct empClubProduct, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/emp/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    empClubProduct.ImageUrl = newFilenameUrl;
                }
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/emp/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload2.SaveAs(physicalFilename);
                    empClubProduct.FileUrl = newFilenameUrl;
                }
                #endregion
                empClubProduct.IsDeleted=false;
				empClubProduct.CreationDate= DateTime.Now; 
                empClubProduct.Id = Guid.NewGuid();
                db.EmpClubProducts.Add(empClubProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpClubProductGroupId = new SelectList(db.EmpClubProductGroups, "Id", "Title");

            return View(empClubProduct);
        }

        // GET: EmpClubProducts/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProduct empClubProduct = db.EmpClubProducts.Find(id);
            if (empClubProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpClubProductGroupId = new SelectList(db.EmpClubProductGroups, "Id", "Title",empClubProduct.EmpClubProductGroupId);

            return View(empClubProduct);
        }

        // POST: EmpClubProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmpClubProduct empClubProduct, HttpPostedFileBase fileupload, HttpPostedFileBase fileupload2)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/emp/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload.SaveAs(physicalFilename);
                    empClubProduct.ImageUrl = newFilenameUrl;
                }
                if (fileupload2 != null)
                {
                    string filename = Path.GetFileName(fileupload2.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    string newFilenameUrl = "/Uploads/emp/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);
                    fileupload2.SaveAs(physicalFilename);
                    empClubProduct.FileUrl = newFilenameUrl;
                }
                #endregion
                empClubProduct.IsDeleted=false;
                db.Entry(empClubProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpClubProductGroupId = new SelectList(db.EmpClubProductGroups, "Id", "Title",empClubProduct.EmpClubProductGroupId);
            return View(empClubProduct);
        }

        // GET: EmpClubProducts/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubProduct empClubProduct = db.EmpClubProducts.Find(id);
            if (empClubProduct == null)
            {
                return HttpNotFound();
            }
            return View(empClubProduct);
        }

        // POST: EmpClubProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmpClubProduct empClubProduct = db.EmpClubProducts.Find(id);
			empClubProduct.IsDeleted=true;
			empClubProduct.DeletionDate=DateTime.Now;
 
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
