using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class OrdersController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]

        // GET: Orders
        public ActionResult Index(string type)
        {
            List<Order> orders = new List<Order>();
            if (type == "all")
                orders = db.Orders.Include(o => o.City).Where(o => o.IsDeleted == false)
                    .Include(o => o.OrderType).Include(o => o.User).OrderByDescending(o => o.CreationDate).ToList();

            if (type == "site")
                orders = db.Orders.Include(o => o.City).Where(o => o.IsSiteOrder == true && o.IsPaid && o.IsDeleted == false)
                    .Include(o => o.OrderType).Include(o => o.User).OrderByDescending(o => o.CreationDate).ToList();


            if (type == "app")
                orders = db.Orders.Include(o => o.City).Where(o => o.IsSiteOrder != true && o.IsPaid && o.IsDeleted == false)
                    .Include(o => o.OrderType).Include(o => o.User).OrderByDescending(o => o.CreationDate).ToList();


            ViewBag.Sum = orders.Where(c => c.IsPaid).Sum(c => c.TotalAmount).ToString("N0")+ " تومان";


            List<OrderListViewModel> orderList=new List<OrderListViewModel>();

            foreach (Order order in orders)
            {
                string productTitle = "";
                OrderDetail orderDetail = db.OrderDetails.FirstOrDefault(c => c.OrderId == order.Id);
                if (orderDetail != null)
                {
                    Product product = db.Products.FirstOrDefault(c => c.Id == orderDetail.ProductId);

                    if (product != null)
                        productTitle = product.Title;
                }

                orderList.Add(new OrderListViewModel()
                {
                    Id=order.Id,
                    Code = order.Code,
                    IsPaid = order.IsPaid,
                    Amount = order.AmountStr,
                    TotalAmount = order.TotalAmountStr,
                    FullName = order.User.FullName,
                    CreationDate = order.CreationDate,
                    PaymentDate = order.PaymentDate,
                    OrderTypeTitle = order.OrderType.Title,
                    ResCode = order.RefId,
                    ProductTitle = productTitle
                });
            }

            return View(orderList);
        }


        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title");
            ViewBag.OrderTypeId = new SelectList(db.ProductTypes.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title");
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "FullName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsDeleted = false;
                order.CreationDate = DateTime.Now;
                order.Id = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.CityId);
            ViewBag.OrderTypeId = new SelectList(db.ProductTypes.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.OrderTypeId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "FullName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.CityId);
            ViewBag.OrderTypeId = new SelectList(db.ProductTypes.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.OrderTypeId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "FullName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                order.IsDeleted = false;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.CityId);
            ViewBag.OrderTypeId = new SelectList(db.ProductTypes.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "Title", order.OrderTypeId);
            ViewBag.UserId = new SelectList(db.Users.Where(current => current.IsDeleted == false && current.IsActive == true), "Id", "FullName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            order.IsDeleted = true;
            order.DeletionDate = DateTime.Now;

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
