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
    public class ProductsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();

        [Route("video/{productGroup}")]
        public ActionResult GetVideos(string productGroup)
        {
            Guid typeId = new Guid("800AD0E8-A281-4C15-AAB1-1BC9D883B8DD");

            ProductGroup oProductGroup = UnitOfWork.ProductGroupRepository
                .Get(current => current.UrlParam == productGroup).FirstOrDefault();

            if (oProductGroup == null)
                return RedirectPermanent("/video");

            VideoListViewModel videos = new VideoListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Products = UnitOfWork.ProductRepository.Get(current => current.ProductGroup.UrlParam == productGroup && current.ProductTypeId == typeId).ToList(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
                ProductGroupTitle = oProductGroup.Title,
                ProductGroupUrlParam = oProductGroup.UrlParam,
                SideBarProducts = GetHomeProducts()
            };
            return View(videos);
        }


        [Route("video/{productGroupUrlParam}/{videoCode}")]
        public ActionResult GetVideoDetail(string productGroupUrlParam, string videoCode)
        {
            int code = Convert.ToInt32(videoCode.Split('-')[1]);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product == null)
                return RedirectPermanent("/video");

            if (product.ProductGroup.UrlParam != productGroupUrlParam)
                return RedirectPermanent("/video/" + product.ProductGroup.UrlParam + "/g-" + product.Code);


            VideoDetailViewModel video = new VideoDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault(),
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(video);
        }


        [Route("forms")]
        public ActionResult GetForms()
        {
            Guid typeId = new Guid("E71DF968-54A3-497A-A8C2-1058A4293F8D");

            ProductListViewModel products = new ProductListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Products = UnitOfWork.ProductRepository.Get(current => current.ProductTypeId == typeId).ToList(),
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }

        [Route("forms/{productCode}")]
        public ActionResult GetFormDetail(string productCode)
        {
            int code = Convert.ToInt32(productCode.Split('-')[1]);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product == null)
                return RedirectPermanent("/forms");

            FormDetailViewModel form = new FormDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault(),
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(form);
        }

        [Route("products")]
        public ActionResult GetProducts()
        {
            Guid typeId = new Guid("BAFB5CA2-1212-4E9D-9F43-79954C8F303F");

            ProductListViewModel products = new ProductListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                SideBarProducts = GetHomeProducts(),
                Products = UnitOfWork.ProductRepository.Get(current => current.ProductTypeId == typeId).ToList(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }

        [Route("products/{productCode}")]
        public ActionResult GetProductDetail(string productCode)
        {
            int code = Convert.ToInt32(productCode.Split('-')[1]);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product == null)
                return RedirectPermanent("/products");

            FormDetailViewModel form = new FormDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault(),
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(form);
        }

        [Route("online-course")]
        public ActionResult GetOnlineCourses()
        {
            Guid typeId = new Guid("408E7974-EF5C-46DF-9456-3AAC7635B16F");

            ProductListViewModel products = new ProductListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Products = UnitOfWork.ProductRepository.Get(current => current.ProductTypeId == typeId).ToList(),
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }

        [Route("online-course/{productCode}")]
        public ActionResult GetOnlineCourseDetail(string productCode)
        {
            int code = Convert.ToInt32(productCode.Split('-')[1]);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product == null)
                return RedirectPermanent("/online-course");

            FormDetailViewModel form = new FormDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                SideBarProducts = GetHomeProducts(),
                Product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(form);
        }




        [Route("promotion")]
        public ActionResult GetPromotion()
        {
            ProductListViewModel products = new ProductListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Products = UnitOfWork.ProductRepository.Get(current => current.IsInPromotion).ToList(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }



        [Route("promotiontype/eydtaeyd")]
        public ActionResult GetEydTaEydPromotion()
        {
            ProductListViewModel products = new ProductListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Products = UnitOfWork.ProductRepository.Get(current => current.IsInPromotion && current.IsPrevious != true).ToList(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }



        [Route("promotion/{productCode}")]
        public ActionResult GetPromotionDetail(string productCode)
        {
            int code = Convert.ToInt32(productCode.Split('-')[1]);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product == null)
                return RedirectPermanent("/promotion");

            string groupUrlParam = "forms";

            if (product.ProductTypeId == new Guid("E71DF968-54A3-497A-A8C2-1058A4293F8D"))
                groupUrlParam = "forms";

            else if (product.ProductTypeId == new Guid("BAFB5CA2-1212-4E9D-9F43-79954C8F303F"))
                groupUrlParam = "products";

            else if (product.ProductTypeId == new Guid("408E7974-EF5C-46DF-9456-3AAC7635B16F"))
                groupUrlParam = "online-course";

            return RedirectPermanent("/" + groupUrlParam + "/" + productCode);
        }


        public List<HomeProducts> GetHomeProducts()
        {

            List<Product> products = UnitOfWork.ProductRepository.Get(current =>
                current.IsInHome && current.IsFree == false &&
                (current.ProductType.Name == "forms" || current.ProductType.Name == "course" ||
                 current.ProductType.Name == "physicalproduct")).Take(8).ToList();

            List<HomeProducts> homeProducts = new List<HomeProducts>();

            foreach (Product product in products)
            {


                homeProducts.Add(new HomeProducts()
                {
                    DiscountAmount = product.DiscountAmount?.ToString("N0") + " تومان",
                    Amount = product.Amount.ToString("N0") + " تومان",
                    ImageUrl = "https://ghanongostar.zavoshsoftware.com/" + product.ImageUrl,
                    IsInPromotion = product.IsInPromotion,
                    Title = product.Title,
                    Url = GetProductUrlPrefix(product.ProductTypeId) + "/g-" + product.Code
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