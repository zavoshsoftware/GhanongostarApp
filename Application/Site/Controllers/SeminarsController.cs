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
    public class SeminarsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private BaseViewModelHelper _baseHelper = new BaseViewModelHelper();



        [Route("seminar/{isOld}")]
        public ActionResult GetSeminars(string isOld)
        {
            bool isNew = true;
            if (isOld.ToLower() == "old")
                isNew = false;

            SeminarListViewModel products = new SeminarListViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),

                Seminars = UnitOfWork.SeminarRepository.Get(current => current.IsNew == isNew && current.IsActive)
                    .ToList(),

                SideBarProducts = GetHomeProducts(),

                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(products);
        }

        [Route("seminar/detail/{id:Guid}")]
        public ActionResult GetSeminarDetail(Guid id)
        {
    

            Seminar seminar = UnitOfWork.SeminarRepository.Get(current => current.Id == id).FirstOrDefault();

            if (seminar == null)
                return RedirectPermanent("/seminar/old");

            SeminarDetailViewModel result = new SeminarDetailViewModel()
            {
                MenuProductGroups = _baseHelper.GetMenuProductGroups(),
                Seminar = seminar,
                SideBarProducts = GetHomeProducts(),
                SideBarProductGroups = _baseHelper.GetSidebarProductGroups(),
            };
            return View(result);
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

        public void TransferBlogImagesToSeminar()
        {
            var seminars = UnitOfWork.SeminarRepository.Get().ToList();

            foreach (var seminar in seminars)
            {
                if (!string.IsNullOrEmpty(seminar.Description))
                {

                    Guid blogid = new Guid(seminar.Description);

                    var blogImages = UnitOfWork.SiteBlogImageRepository.Get(c => c.SiteBlogId == blogid && c.IsActive)
                        .ToList();

                    foreach (var image in blogImages)
                    {
                        SeminarImage seminarImage = new SeminarImage()
                        {
                            Id = Guid.NewGuid(),
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false,
                            ImageUrl = image.ImageUrl,
                            SeminarId = seminar.Id
                        };

                        UnitOfWork.SeminarImageRepository.Insert(seminarImage);
                        UnitOfWork.Save();

                    }
                }
            }
        }

    }
}