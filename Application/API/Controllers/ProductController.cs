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
using System.Data.Entity;

namespace API.Controllers
{
    public class ProductController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        [Route("Product/List")]
        [HttpPost]
        [Authorize]
        public ProductLisViewModel PostProductList(ProductListInputViewModel input)
        {
            ProductLisViewModel result = new ProductLisViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                result.Result = GetProductList(input.ProductTypeName, user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

                UnitOfWork.Save();
                CountProductList(input.ProductTypeName);

                return result;
            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }

        public void CountProductList(string typeName)
        {
            switch (typeName)
            {
                case "forms":
                    {
                        pageCounter.Count("formlist", null);
                        break;
                    }
                case "course":
                    {
                        pageCounter.Count("courselist", null);
                        break;
                    }
                case "physicalproduct":
                    {
                        pageCounter.Count("productpackagelist", null);
                        break;
                    }
                case "event":
                    {
                        pageCounter.Count("eventslist", null);
                        break;
                    }
                case "question":
                    {
                        pageCounter.Count("question", null);
                        break;
                    }
                case "workshop":
                    {
                        pageCounter.Count("workshoplist", null);
                        break;
                    }
                case "Vippackage":
                    {
                        pageCounter.Count("vip", null);
                        break;
                    }
            }
        }

        public void CountProductDetail(Product product)
        {
            switch (product.ProductType.Name)
            {
                case "forms":
                    {
                        pageCounter.Count("formdetaile", product.Id);
                        break;
                    }
                case "course":
                    {
                        pageCounter.Count("coursedetail", product.Id);
                        break;
                    }
                case "physicalproduct":
                    {
                        pageCounter.Count("productpackagedetail", product.Id);
                        break;
                    }
                case "event":
                    {
                        pageCounter.Count("eventdetail", product.Id);
                        break;
                    }

                case "workshop":
                    {
                        pageCounter.Count("workshopdetail", product.Id);
                        break;
                    }

                case "latest":
                    {
                        pageCounter.Count("videodetail", product.Id);
                        break;
                    }
            }
        }

        PageCounter pageCounter = new PageCounter();

        [Route("Product/ListByGroup")]
        [HttpPost]
        [Authorize]
        public ProductLisViewModel PostProductListByGroup(ProductListByGroupInputViewModel input)
        {
            ProductLisViewModel result = new ProductLisViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                result.Result = GetProductListByGroup(input.ProductTypeName, input.ProductGroupCode, user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

                UnitOfWork.Save();
                pageCounter.Count("videolistbygroup", GetProductGroupIdByCode(input.ProductGroupCode));

                return result;
            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }

        [Route("Product/Detail")]
        [HttpPost]
        [Authorize]
        public ProductDetailViewModel PostProductDetail(ProductDetailInputViewModel input)
        {
            ProductDetailViewModel result = new ProductDetailViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                int productCode = Convert.ToInt32(input.ProductCode);

                Product product =
                    UnitOfWork.ProductRepository.Get(current => current.Code == productCode).FirstOrDefault();

                result.Result = GetProductDetail(product);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

                UnitOfWork.Save();
                CountProductDetail(product);

                return result;
            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }

        [Route("Product/List/questionPackage")]
        [HttpPost]
        [Authorize]
        public QuestionPackageLisViewModel PostQuestionPackageList()
        {
            QuestionPackageLisViewModel result = new QuestionPackageLisViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                result.Result = GetQuestionPackages(user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

                UnitOfWork.Save();

                return result;
            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }

        [Route("Product/UserList")]
        [HttpPost]
        [Authorize]
        public ProductUserListViewModel PostProductUserList()
        {
            ProductUserListViewModel result = new ProductUserListViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }

                result.Result = GetUserProductList(user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);

                UnitOfWork.Save();

                return result;
            }
            catch (Exception e)
            {
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }


        #region Product/List/questionPackage

        public QuestionPackageListViewModel GetQuestionPackages(Guid userId)
        {
            QuestionPackageListViewModel package = new QuestionPackageListViewModel();

            Order order = UnitOfWork.OrderRepository.Get(current =>
                current.UserId == userId && current.OrderType.Name == "question" && current.IsActive && current.ExpireNumber > 0 && current.IsPaid == true).FirstOrDefault();

            if (order == null)
            {
                package.HasPackage = "false";
                package.QuestionNumber = "0";

                List<Product> products =
                    UnitOfWork.ProductRepository.Get(current => current.ProductType.Name == "question").ToList();

                List<QuestionPackageItemViewModel> questionPackages = new List<QuestionPackageItemViewModel>();

                foreach (Product product in products)
                {
                    questionPackages.Add(new QuestionPackageItemViewModel()
                    {
                        Title = product.Title,
                        Amount = product.Amount.ToString(),
                        Description = product.Body,
                        Code = product.Code.ToString()
                    });
                }

                package.QuestionPackages = questionPackages;
            }
            else
            {
                package.HasPackage = "true";
                package.QuestionNumber = order.ExpireNumber.ToString();
            }

            return package;
        }

        #endregion
        #region Detail

        public ProductDetailItemViewModel GetProductDetail(Product product)
        {


            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

            string videoThumUrl = null;
            if (!string.IsNullOrEmpty(product.VideoThumbnail))
                videoThumUrl = baseUrlPanel + product.VideoThumbnail;

            string fileUrl = null;
            if (!string.IsNullOrEmpty(product.FileUrl))
                fileUrl = baseUrlPanel + product.FileUrl;


            decimal amount = product.Amount;

            if (product.IsInPromotion && product.DiscountAmount != null)
                amount = product.DiscountAmount.Value;

            ProductDetailItemViewModel productDetail = new ProductDetailItemViewModel()
            {
                Image = baseUrlPanel + product.ImageUrl,
                IsFree = product.IsFree.ToString(),
                Point = product.Point.ToString(),
                Amount = amount.ToString(),
                Title = product.Title,
                webViewUrl = baseUrlPanel + "Product/detail/" + product.Id,
                ProductFileUrl = fileUrl,
                VideoThumb = videoThumUrl
            };

            return productDetail;
        }


        #endregion

        #region List 
        public List<ProductUserListItemViewModel> GetUserProductList(Guid id)
        {
            List<ProductUserListItemViewModel> productList = new List<ProductUserListItemViewModel>();
            List<Order> userOrders = UnitOfWork.OrderRepository.Get(current => current.UserId == id).Include(current => current.OrderType).ToList();
            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];
            foreach (Order order in userOrders)
            {
                //ProductType type = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId);

                if (order.OrderType.Name != "question")
                {
                    OrderDetail orderDetail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id && current.IsDeleted == false).Include(current => current.Product).FirstOrDefault();

                    if (orderDetail.Product.IsDeleted == false)
                    {
                        productList.Add(new ProductUserListItemViewModel()
                        {
                            Image = baseUrlPanel + orderDetail.Product.ImageUrl,
                            IsFree = orderDetail.Product.IsFree.ToString(),
                            Point = orderDetail.Product.Point.ToString(),
                            Amount = orderDetail.Product.Amount.ToString(),
                            Title = orderDetail.Product.Title,
                            Code = orderDetail.Product.Code.ToString(),
                            Ispreviously = GetIsPrevously(orderDetail.Product.IsPrevious),
                            ProductTypeTitle = orderDetail.Product.ProductType.Title,
                            ProductTypeName = orderDetail.Product.ProductType.Name
                        });
                    }

                }
            }
            return productList;
        }

        public Guid? GetProductGroupIdByCode(string code)
        {
            int productGroupCode = Convert.ToInt32(code);
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository
                .Get(current => current.Code == productGroupCode).FirstOrDefault();

            if (productGroup != null)
                return productGroup.Id;

            return null;
        }
        public List<ProductListItemViewModel> GetProductListByGroup(string productTypeName, string productGroupCode, Guid userId)
        {
            List<ProductListItemViewModel> productList = new List<ProductListItemViewModel>();

            Guid? producGroupCodeGuid = GetProductGroupIdByCode(productGroupCode);

            List<Product> products =
                UnitOfWork.ProductRepository.Get(current =>
                    current.ProductType.Name == productTypeName && current.IsActive &&
                    current.ProductGroupId == producGroupCodeGuid).ToList();

            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

            bool isOwner = false;
            foreach (Product product in products)
            {
                isOwner = false;
                List<Order> orders = UnitOfWork.OrderRepository
                    .Get(current => current.UserId == userId && current.OrderTypeId == product.ProductTypeId && current.IsActive == true).OrderBy(current => current.CreationDate).ToList();
                foreach (Order order in orders)
                {
                    if (order.IsPaid)
                    {
                        OrderDetail detail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id && current.ProductId == product.Id && current.Product.IsFree == false && current.IsActive == true).FirstOrDefault();


                        if (detail != null)
                            isOwner = true;
                    }
                    else

                    {
                        OrderDetail detail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id && current.ProductId == product.Id && current.Product.IsFree == true).FirstOrDefault();


                        if (detail != null)
                            isOwner = true;
                    }



                }


                decimal amount = product.Amount;

                if (product.IsInPromotion && product.DiscountAmount != null)
                    amount = product.DiscountAmount.Value;


                productList.Add(new ProductListItemViewModel()
                {
                    Image = baseUrlPanel + product.ImageUrl,
                    IsFree = product.IsFree.ToString(),
                    Point = product.Point.ToString(),
                    Amount = amount.ToString(),
                    Title = product.Title,
                    Code = product.Code.ToString(),
                    IsOwner = isOwner,
                    Ispreviously = GetIsPrevously(product.IsPrevious),
                    TypeName = product.ProductType.Name
                });

            }
            return productList.OrderByDescending(current => current.IsOwner).ToList();
        }


        public List<ProductListItemViewModel> GetProductList(string productTypeName, Guid userId)
        {
            List<ProductListItemViewModel> productList = new List<ProductListItemViewModel>();

            List<Product> products =
                UnitOfWork.ProductRepository.Get(current => current.ProductType.Name == productTypeName && current.IsActive == true).ToList();

            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

            bool isOwner = false;
            foreach (Product product in products)
            {
                isOwner = false;
                List<Order> orders = UnitOfWork.OrderRepository
                    .Get(current => current.UserId == userId && current.OrderTypeId == product.ProductTypeId && current.IsActive == true).OrderBy(current => current.CreationDate).ToList();
                foreach (Order order in orders)
                {
                    if (order.IsPaid)
                    {
                        OrderDetail detail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id && current.ProductId == product.Id && current.Product.IsFree == false && current.IsActive == true).FirstOrDefault();


                        if (detail != null)
                            isOwner = true;
                    }
                    else

                    {
                        OrderDetail detail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id && current.ProductId == product.Id && current.Product.IsFree == true).FirstOrDefault();


                        if (detail != null)
                            isOwner = true;
                    }



                }
                decimal amount = product.Amount;

                if (product.IsInPromotion && product.DiscountAmount != null)
                    amount = product.DiscountAmount.Value;

                productList.Add(new ProductListItemViewModel()
                {
                    Image = baseUrlPanel + product.ImageUrl,
                    IsFree = product.IsFree.ToString(),
                    Point = product.Point.ToString(),
                    Amount = amount.ToString(),
                    Title = product.Title,
                    Code = product.Code.ToString(),
                    IsOwner = isOwner,
                    Ispreviously = GetIsPrevously(product.IsPrevious),
                    TypeName = product.ProductType.Name
                });

            }
            return productList.OrderByDescending(current => current.IsOwner).ToList();
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

        #endregion
    }
}