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
using ViewModels;

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
            EmpClubQuestionViewModel model = new EmpClubQuestionViewModel();
            model.UserQuestion = new EmpClubQuestion();
            model.UserQuestion = db.EmpClubQuestions.Find(id);
            model.UserQuestions = new List<EmpClubQuestion>();
            model.UserQuestions = db.EmpClubQuestions.Where(x => x.UserId == model.UserQuestion.UserId && x.Id != id).OrderByDescending(c=>c.CreationDate)
                .ToList();


            if (model.UserQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", model.UserQuestion.UserId);
            ViewBag.status = status;
            return View(model);
        }

        // POST: EmpClubQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmpClubQuestionViewModel empClubQuestion, string status)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(empClubQuestion.UserQuestion.Response))
                    empClubQuestion.UserQuestion.ResponseDate = DateTime.Now;

                empClubQuestion.UserQuestion.IsDeleted = false;
                db.Entry(empClubQuestion).State = EntityState.Modified;
                db.SaveChanges();
                if (string.IsNullOrEmpty(status))
                    return RedirectToAction("Index");
                return RedirectToAction("Index", new { status = "notanswered" });
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "Password", empClubQuestion.UserQuestion.UserId);
            return View(empClubQuestion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveVoiceAnswer(Guid id, HttpPostedFile questionVoice)
        {
            string FilePath = Path.Combine(Server.MapPath("~/uloadfolder"), questionVoice.FileName);
            string FileUrl = Url.Content(Path.Combine("~/uloadfolder", questionVoice.FileName));
            questionVoice.SaveAs(FilePath);
            return View("Edit",new {id=id });
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
