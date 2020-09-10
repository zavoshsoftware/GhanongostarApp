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
    public class UsersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Parent).Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.CreationDate).Include(u => u.Role).Where(u=>u.IsDeleted==false).OrderByDescending(u=>u.CreationDate);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Users, "Id", "Password");
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Password,CellNum,FullName,Code,RoleId,ParentId,Token,RemainCredit,OsType,VersionNumber,IsVip,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Code = ReturnCode();
                user.IsDeleted=false;
				user.CreationDate= DateTime.Now; 
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.Users, "Id", "Password", user.ParentId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(db.Users, "Id", "Password", user.ParentId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Password,CellNum,FullName,Code,RoleId,ParentId,Token,RemainCredit,OsType,VersionNumber,IsVip,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] User user)
        {
            if (ModelState.IsValid)
            {
				user.IsDeleted=false;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Users, "Id", "Password", user.ParentId);
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Title", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            User user = db.Users.Find(id);
			user.IsDeleted=true;
			user.DeletionDate=DateTime.Now;
 
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



        public Guid ReturnUserRole()
        {
            Guid roleId = new Guid("5812d0c0-264f-4a9b-96bb-42dfc70538e6");
            return roleId;
        }

        public int ReturnCode()
        {
            Guid userRoleId = ReturnUserRole();
            User user = db.Users.Where(current => current.RoleId == userRoleId).OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 300001;
            }
        }
    }
}
