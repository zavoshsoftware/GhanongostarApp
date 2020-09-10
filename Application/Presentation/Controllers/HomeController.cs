using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        [Authorize(Roles = "SuperAdministrator")]
        // GET: Home
       
        public ActionResult Index()
        {
            HomeViewModel home = new HomeViewModel();
            home.UserCount = db.Users.Where(current => current.IsDeleted == false && current.IsActive == true).Count();
            home.FomrsCount = db.Products.Where(current => current.IsDeleted == false && current.IsActive == true && current.ProductType.Name.ToLower() == "forms").Count();
            home.ProductCount= db.Products.Where(current => current.IsDeleted == false && current.IsActive == true && current.ProductType.Name.ToLower() == "physicalproduct").Count();
            home.OrderCount = db.Orders.Where(current => current.IsDeleted == false && current.IsActive == true).Count();
            return View(home);
        }
    }
}