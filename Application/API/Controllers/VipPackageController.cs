using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Helper;
using Models;
using Models.Input;

namespace API.Controllers
{
    public class VipPackageController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: VipPackage
        readonly StatusManagement status = new StatusManagement();

        [Route("VipPackages/Get")]
        [HttpPost]
        [Authorize]
        public VipPackageViewModel Get()
        {
            VipPackageViewModel result = new VipPackageViewModel();
            //try
            //{
            string token = GetRequestHeader();
            User user = UnitOfWork.UserRepository.GetByToken(token);
            if (user.IsActive == false)
            {
                result.Result = null;
                result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                return result;
            }
            result.Result = GetVipPackageItems(user.Id);
            result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

            return result;

            //}
            //catch (Exception e)
            //{
            //    var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
            //        e.ToString());

            //    result.Result = null;
            //    result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
            //    return result;
            //}
        }
        public VipPackageItem GetVipPackageItems(Guid id)
        {
            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];
            VipPackageItem item = new VipPackageItem();

            List<ProductListItemViewModel> productList = new List<ProductListItemViewModel>();
            List<Product> products = UnitOfWork.ProductRepository.Get(current => current.IsVip == true).ToList();

            bool isOwner = false;

            foreach (Product product in products)
            {
                isOwner = false;
                Order order = UnitOfWork.OrderRepository
                    .Get(current => current.UserId == id && current.OrderTypeId == product.ProductTypeId).FirstOrDefault();


                if (order != null)
                    isOwner = true;

                productList.Add(new ProductListItemViewModel()
                {
                    Image = baseUrlPanel + product.ImageUrl,
                    IsFree = product.IsFree.ToString(),
                    Point = product.Point.ToString(),
                    Amount = product.Amount.ToString(),
                    Title = product.Title,
                    Code = product.Code.ToString(),
                    IsOwner = isOwner,
                    Ispreviously = GetIsPrevously(product.IsPrevious),
                    TypeName = product.ProductType.Name


                });
            }

            List<VipPackage> vipPackages = UnitOfWork.VipPackageRepository.Get().ToList();
            List<VipPackageListItem> vipPackageList = new List<VipPackageListItem>();
            
            foreach (VipPackage package in vipPackages)
            {
               
                Product product = UnitOfWork.ProductRepository.GetById(package.ProductId);

                vipPackageList.Add(new VipPackageListItem()
                {
                    Month = package.Duration,
                    Price = package.Price,
                    ProductCode = product.Code,
                    Title = package.Title,
                    Features = ReturnFeatures(package.Id)


                });

                
            }
            item.HasVipPackage = UnitOfWork.UserRepository.GetById(id).IsVip;
            item.VipPackageList = vipPackageList;
            item.Products = productList;

            return item;


        }
        public List<string> ReturnFeatures(Guid id)
        {
            List<string> packageFeatureItems = new List<string>();
            List<VipPackageFeature> features = UnitOfWork.VipPackageFeatureRepository.Get(current => current.VipPackageId ==id).ToList();
            foreach (VipPackageFeature feature in features)
            {
                packageFeatureItems.Add(feature.Title);
            }
            return packageFeatureItems;
        }
        public bool GetIsPrevously(bool? isPrevious)
        {
            bool isProviously = false;

            if (isPrevious != null)
                if (isPrevious.Value)
                    isProviously = true;


            return isProviously;
        }
        public string GetRequestHeader()
        {
            var re = Request;
            var headers = re.Headers;

            if (headers.Contains("Authorization"))
            {
                string token = headers.GetValues("Authorization").First();
                return token;
            }
            return String.Empty;
        }
    }
}