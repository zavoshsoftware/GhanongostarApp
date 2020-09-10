using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace Site.Controllers
{
    public class OrdersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();

        [Authorize]
        public ActionResult List()
        {
            User user = GetOnlineUser();

            OrderListViewModel orders = new OrderListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                User = user,
                Orders = UnitOfWork.OrderRepository.Get(c => c.UserId == user.Id).OrderByDescending(c => c.CreationDate).ToList()
            };

            return View(orders);
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            Order order = UnitOfWork.OrderRepository.Get(c => c.Code == id).FirstOrDefault();

            OrderDetailViewModel orderDetail = new OrderDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Order = order,
                OrderDetails = UnitOfWork.OrderDetailRepository.Get(c => c.OrderId == order.Id).Include(current => current.Product).ToList()
            };


            OrderDetail od = orderDetail.OrderDetails.FirstOrDefault();

            string orderType = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId).Name;

     

            if (orderType.ToLower() == "course")
            {
                Guid proId = od.ProductId;

                CourseDetail courseDetail = UnitOfWork.CourseDetailRepository
                    .Get(c => c.ProductId == proId).OrderBy(c => c.SessionNumber)
                    .FirstOrDefault();

                if (courseDetail != null)
                {
                    ViewBag.fileLink = "https://ghanongostar.zavoshsoftware.com/" +
                                       courseDetail.VideoUrl;
                }
            }

            return View(orderDetail);
        }

        public User GetOnlineUser()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string name = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;
            Guid userId = new Guid(name);


            return UnitOfWork.UserRepository.GetById(userId);
        }

    }
}