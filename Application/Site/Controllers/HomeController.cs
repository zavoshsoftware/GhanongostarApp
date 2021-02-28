using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace Site.Controllers
{
    public class HomeController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();

        [Route("")]
        public ActionResult Index()
        {
            Guid typeId = new Guid("800AD0E8-A281-4C15-AAB1-1BC9D883B8DD");

            HomeViewModel home = new HomeViewModel()
            {
                SiteBlogs = UnitOfWork.SiteBlogRepository.Get().OrderByDescending(current=>current.CreationDate).Take(3).ToList(),
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                HomeProducts = GetHomeProducts(),
                LatestVideos = UnitOfWork.ProductRepository.Get(current => current.ProductTypeId == typeId&&current.IsActive).OrderByDescending(current => current.CreationDate).Take(3).ToList(),
            };
            return View(home);
        }

        [Route("Consultant")]
        public ActionResult Consultant()
        {
            ConsultantViewModel result = new ConsultantViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
            };
            return View(result);
        }


        [Route("Consultant")]
        [HttpPost]
        public ActionResult Consultant(ConsultantViewModel consultantViewModel)
        {
            if (ModelState.IsValid)
            { 
                ConsultantRequest consultantRequest=new ConsultantRequest()
                {
                    Id = Guid.NewGuid(),
                    Company = consultantViewModel.Company  ,
                    CreationDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    FirstName = consultantViewModel.FirstName,
                    LastName = consultantViewModel.LastName,
                    Message = consultantViewModel.Message,
                    ContactNumber = consultantViewModel.ContactNumber
                };

                UnitOfWork.ConsultantRequestRepository.Insert(consultantRequest);
                UnitOfWork.Save();

                TempData["success"] = "درخواست شما با موفقیت ثبت گردید. همکاران ما در اسرع وقت به درخواست شما رسیدگی خواهند کرد..";
                consultantViewModel.MenuProductGroups = _baseHelper.GetMenuProductGroups();
                return View(consultantViewModel);
            }

            TempData["error"] = "لطفا فیلدهای ستاره دار را تکمیل کنید..";

            ConsultantViewModel result = new ConsultantViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
            };
            return View(result);
        }


        public List<HomeProducts> GetHomeProducts()
        {

            List<Product> products = UnitOfWork.ProductRepository.Get(current =>
                current.IsInHome && current.IsFree == false &&
                (current.ProductType.Name == "forms" || current.ProductType.Name == "course" ||
                current.ProductType.Name == "workshop" || current.ProductType.Name == "physicalproduct")).Take(8).ToList();

            List<HomeProducts> homeProducts=new List<HomeProducts>();

            foreach (Product product in products)
            {
               

                homeProducts.Add(new HomeProducts()
                {
                    DiscountAmount = product.DiscountAmount?.ToString("N0")+ " تومان",
                    Amount = product.Amount.ToString("N0") + " تومان",
                    ImageUrl = "https://ghanongostar.zavoshsoftware.com/" + product.ImageUrl,
                    IsInPromotion = product.IsInPromotion,
                    Title = product.Title,
                    Url = GetProductUrlPrefix(product.ProductTypeId)+"/g-"+product.Code
                });
            }

            return homeProducts;
        }

        public string GetProductUrlPrefix(Guid productGroupId)
        {
            string urlPrefix = "/forms";

            if (productGroupId == new Guid("408E7974-EF5C-46DF-9456-3AAC7635B16F"))
                urlPrefix = "/online-course";
            if (productGroupId == new Guid("BAFB5CA2-1212-4E9D-9F43-79954C8F303F"))
                urlPrefix = "/products";

            return urlPrefix;
        }

      
    }
}