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
    public class SiteBlogsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();

        [Route("blog/{urlParam}")]
        public ActionResult List(string urlParam)
        {
            SiteBlogCategory siteBlogCategory = UnitOfWork.SiteBlogCategoryRepository
                .Get(current => current.UrlParam == urlParam).FirstOrDefault();

            if (siteBlogCategory == null)
                return RedirectPermanent("/blog");

            BlogListViewModel blogs =new BlogListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                SiteBlogs = UnitOfWork.SiteBlogRepository.Get(current=>current.SiteBlogCategory.UrlParam== urlParam).ToList(),
                SideBarProductGroups = _baseHelper.GetMenuProductGroups(),
                SiteBlogCategory = siteBlogCategory
            };
            return View(blogs);
        }

        [Route("blog")]
        public ActionResult PureList()
        {
            //SiteBlogCategory siteBlogCategory = UnitOfWork.SiteBlogCategoryRepository
            //    .Get().FirstOrDefault();

            BlogListViewModel blogs =new BlogListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                SiteBlogs = UnitOfWork.SiteBlogRepository.Get().Include(c => c.SiteBlogCategory).ToList(),
                SideBarProductGroups = _baseHelper.GetMenuProductGroups(),
                //SiteBlogCategory = siteBlogCategory
            };
            return View(blogs);
        }

        [Route("blog/{blogGroupUrlParam}/{urlParam}")]
        public ActionResult Details(string blogGroupUrlParam, string urlParam)
        {
            SiteBlog siteBlog = UnitOfWork.SiteBlogRepository.Get(current => current.UrlParam == urlParam)
                .Include(current => current.SiteBlogCategory).FirstOrDefault();

            if (siteBlog == null)
                return RedirectPermanent("/blog");

            if (siteBlog.SiteBlogCategory.UrlParam != blogGroupUrlParam)
                return RedirectPermanent("/blog/" + siteBlog.SiteBlogCategory.UrlParam + "/" + urlParam);

            BlogDetailViewModel blog=new BlogDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                SiteBlog = siteBlog,
                SideBarBlogs = UnitOfWork.SiteBlogRepository.Get().OrderByDescending(current=>current.CreationDate).Take(3).ToList(),
                SideBarSiteBlogCategories = UnitOfWork.SiteBlogCategoryRepository.Get().ToList(),
                SideBarProducts = GetHomeProducts()
            };

            return View(blog);
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