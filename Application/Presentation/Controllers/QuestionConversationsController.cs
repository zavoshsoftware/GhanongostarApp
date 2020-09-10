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
    public class QuestionConversationsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]

        // GET: QuestionConversations
        public ActionResult Index(Guid? id,string type)
        {
            List<QuestionConversation> questionConversations = new List<QuestionConversation>();
            if (id != null)
            {
                questionConversations = db.QuestionConversations.Include(q => q.Parent)
                    .Where(q => q.IsDeleted == false && q.ParentId == id).OrderByDescending(q => q.CreationDate)
                    .Include(q => q.User).Where(q => q.IsDeleted == false).OrderByDescending(q => q.CreationDate)
                    .ToList();

                ViewBag.Title = "فهرست پاسخ ها";
                ViewBag.id = id;
            }
            else
            {
                if (type == null)
                {
                    questionConversations = db.QuestionConversations.Include(q => q.Parent)
                        .Where(q => q.IsDeleted == false && q.ParentId == null).OrderByDescending(q => q.CreationDate)
                        .Include(q => q.User).Where(q => q.IsDeleted == false).ToList();

                    ViewBag.Title = "فهرست همه سوالات";
                }
                else if (type == "answered")
                {
                    List<QuestionConversation> newQuestionConversations = new List<QuestionConversation>();

                    questionConversations = db.QuestionConversations.Include(q => q.Parent)
                        .Where(q => q.IsDeleted == false && q.ParentId == null).OrderByDescending(q => q.CreationDate)
                        .Include(q => q.User).Where(q => q.IsDeleted == false).ToList();

                    foreach (QuestionConversation conversation in questionConversations.ToList())
                    {
                        if (db.QuestionConversations.Any(current => current.ParentId == conversation.Id))
                        {
                            newQuestionConversations.Add(conversation);
                        }
                    }

                    questionConversations = newQuestionConversations;
                    ViewBag.Title = "فهرست سوالات پاسخ داده شده";
                }
                else if (type == "notanswered")
                {
                    questionConversations = db.QuestionConversations.Include(q => q.Parent)
                        .Where(q => q.IsDeleted == false && q.ParentId == null).OrderByDescending(q => q.CreationDate)
                        .Include(q => q.User).Where(q => q.IsDeleted == false).ToList();

                    foreach (QuestionConversation conversation in questionConversations.ToList())
                    {
                        if (db.QuestionConversations.Any(current => current.ParentId == conversation.Id))
                        {
                            questionConversations.Remove(conversation);
                        }
                    }

                    ViewBag.Title = "فهرست سوالات پاسخ داده نشده";
                }
                ViewBag.id = null;
            }
            return View(questionConversations.ToList());
        }

        // GET: QuestionConversations/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionConversation questionConversation = db.QuestionConversations.Find(id);
            if (questionConversation == null)
            {
                return HttpNotFound();
            }
            return View(questionConversation);
        }

        // GET: QuestionConversations/Create
        public ActionResult Create(Guid? id)
        {
            if (id != null)
            {
                ViewBag.ParentId = new SelectList(db.QuestionConversations.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Subject", id);
                ViewBag.id = id;
                QuestionConversation question = db.QuestionConversations.Find(id);
                ViewBag.subject = question.Subject;
                ViewBag.body = question.Body;
            }

            else
            {
                ViewBag.ParentId = new SelectList(db.QuestionConversations.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Subject");
                ViewBag.id = null;
            }

            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Fullname");
            return View();
        }

        // POST: QuestionConversations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(QuestionConversation questionConversation, Guid? id)
        {
            if (ModelState.IsValid)
            {
                QuestionConversation parent = db.QuestionConversations.Find(id);

                if (parent != null)
                    parent.StatusCode = 1;

                questionConversation.Order = GetConversationOrder(id.Value);
                questionConversation.ParentId = id;
                questionConversation.IsDeleted = false;
                questionConversation.CreationDate = DateTime.Now;
                questionConversation.Id = Guid.NewGuid();
                questionConversation.UserId = GetOnlineUserId();
                questionConversation.IsActive = true;

                db.QuestionConversations.Add(questionConversation);
                db.SaveChanges();
                ViewBag.id = questionConversation.ParentId;

                return RedirectToAction("Index", new { id = id });

            }

            ViewBag.ParentId = new SelectList(db.QuestionConversations.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Subject", questionConversation.ParentId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Fullname", questionConversation.UserId);
            return View(questionConversation);
        }

        public int GetConversationOrder(Guid parentId)
        {
            List<QuestionConversation> questionConversations = db.QuestionConversations
                .Where(current => current.ParentId == parentId && current.IsDeleted == false)
                .OrderByDescending(current => current.Order).ToList();

            if (questionConversations.Any())
                return questionConversations.FirstOrDefault().Order + 1;
            else
                return 2;
        }

        public Guid GetOnlineUserId()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            return user.Id;
        }
        // GET: QuestionConversations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionConversation questionConversation = db.QuestionConversations.Find(id);
            if (questionConversation == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = questionConversation.ParentId;
            ViewBag.ParentId = new SelectList(db.QuestionConversations.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Subject", questionConversation.ParentId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Fullname", questionConversation.UserId);
            return View(questionConversation);
        }

        // POST: QuestionConversations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionConversation questionConversation)
        {
            if (ModelState.IsValid)
            {
                if (questionConversation.ParentId != null)
                    questionConversation.StatusCode = 1;
                else
                    questionConversation.StatusCode = 0;
                questionConversation.IsDeleted = false;
                db.Entry(questionConversation).State = EntityState.Modified;
                db.SaveChanges();
                if (questionConversation.ParentId != null)
                    return RedirectToAction("Index", new { id = questionConversation.ParentId });
                else
                    return RedirectToAction("Index");
            }

            ViewBag.id = questionConversation.ParentId;
            ViewBag.ParentId = new SelectList(db.QuestionConversations.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Subject", questionConversation.ParentId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Fullname", questionConversation.UserId);
            return View(questionConversation);
        }

        // GET: QuestionConversations/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionConversation questionConversation = db.QuestionConversations.Find(id);
            if (questionConversation == null)
            {
                return HttpNotFound();
            }
            ViewBag.id = questionConversation.ParentId;
            return View(questionConversation);
        }

        // POST: QuestionConversations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            QuestionConversation questionConversation = db.QuestionConversations.Find(id);
            questionConversation.IsDeleted = true;
            questionConversation.DeletionDate = DateTime.Now;
            ViewBag.id = questionConversation.ParentId;
            db.SaveChanges();
            if (questionConversation.ParentId != null)
                return RedirectToAction("Index", new { id = questionConversation.ParentId });
            else
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

