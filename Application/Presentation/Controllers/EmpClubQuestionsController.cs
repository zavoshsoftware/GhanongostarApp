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
    public class EmpClubQuestionsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: EmpClubQuestions
        public ActionResult Index(string status)
        {
            ViewBag.status = status;
            ViewBag.Title = "فهرست کلیه سوالات باشگاه کارفرمایان";
            if (status == "notanswered")
            {
                var empClubQuestions = db.EmpClubQuestions.Include(e => e.User)
                    .Where(e => (e.Response == null || e.Response == "") && e.IsDeleted == false)
                    .OrderByDescending(e => e.CreationDate);
                ViewBag.Title = "فهرست سوالات پاسخ داده نشده باشگاه کارفرمایان";
                return View(empClubQuestions.ToList());
            }
            else
            {
                var empClubQuestions = db.EmpClubQuestions.Include(e => e.User)
                    .Where(e => e.IsDeleted == false)
                    .OrderByDescending(e => e.CreationDate);
                return View(empClubQuestions.ToList());
            }

        }

        // GET: EmpClubQuestions/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubQuestion empClubQuestion = db.EmpClubQuestions.Find(id);
            if (empClubQuestion == null)
            {
                return HttpNotFound();
            }
            return View(empClubQuestion);
        }

        // GET: EmpClubQuestions/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password");
            return View();
        }

        // POST: EmpClubQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Subject,Question,Response,ResponseDate,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] EmpClubQuestion empClubQuestion)
        {
            if (ModelState.IsValid)
            {
                empClubQuestion.IsDeleted = false;
                empClubQuestion.CreationDate = DateTime.Now;
                empClubQuestion.Id = Guid.NewGuid();
                db.EmpClubQuestions.Add(empClubQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", empClubQuestion.UserId);
            return View(empClubQuestion);
        }

        // GET: EmpClubQuestions/Edit/5
        public ActionResult Edit(Guid? id, string status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubQuestion empClubQuestion = db.EmpClubQuestions.Find(id);
            if (empClubQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", empClubQuestion.UserId);
            ViewBag.status = status;
            return View(empClubQuestion);
        }

        // POST: EmpClubQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmpClubQuestion empClubQuestion, string status)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(empClubQuestion.Response))
                    empClubQuestion.ResponseDate = DateTime.Now;

                empClubQuestion.IsDeleted = false;
                db.Entry(empClubQuestion).State = EntityState.Modified;
                db.SaveChanges();
                if (string.IsNullOrEmpty(status))
                    return RedirectToAction("Index");
                return RedirectToAction("Index", new { status = "notanswered" });
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", empClubQuestion.UserId);
            return View(empClubQuestion);
        }

        // GET: EmpClubQuestions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmpClubQuestion empClubQuestion = db.EmpClubQuestions.Find(id);
            if (empClubQuestion == null)
            {
                return HttpNotFound();
            }
            return View(empClubQuestion);
        }

        // POST: EmpClubQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            EmpClubQuestion empClubQuestion = db.EmpClubQuestions.Find(id);
            empClubQuestion.IsDeleted = true;
            empClubQuestion.DeletionDate = DateTime.Now;

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
