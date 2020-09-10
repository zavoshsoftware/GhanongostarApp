using API.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using Helper;
using Models;
using Models.Input;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.IO;
using System.Drawing;
using System.Web;
using API.Utility;

namespace API.Controllers
{
    public class OrderController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();
        [Route("Order/Post")]
        [HttpPost]
        [Authorize]
        public OrderResultViewModel PostOrder(OrderPostInputViewModel input)
        {
            OrderResultViewModel result = new OrderResultViewModel();
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

                string paymentUrl =
                    GetOrderResult(input.ProductCode, user.Id, input.Email, input.FullName, input.CityId, input.PostalCode, input.Address,input.DiscountCodeId,input.DiscountAmount);

                OrderItemViewModel orderRes = new OrderItemViewModel();
                if (paymentUrl == "Invalid")
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(0, Resources.Messages.InvalidQuestion, false);
                }
                else if (paymentUrl != "false")
                {

                    orderRes.PaymentLink = paymentUrl;
                    result.Result = orderRes;
                    result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);
                }

                else
                {
                    result.Result = orderRes;
                    result.Status = status.ReturnStatus(0, Resources.Messages.InvaliProductId, false);
                }
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

        [Route("Order/Get")]
        [HttpPost]
        [Authorize]
        public SuccessPostViewModel GetOrder()
        {
            SuccessPostViewModel result = new SuccessPostViewModel();
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

                if (user != null)
                {
                    result.Result = user.Email;
                    result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);

                }
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

        #region List   

        public string GetOrderResult(string productCode, Guid userId, string email, List<string> fullName, Guid cityId, string postalCode, string address,Guid? discountId,decimal? discountAmout)
        {
            string paymentUrl = WebConfigurationManager.AppSettings["PaymentUrl"];

            int code = Convert.ToInt32(productCode);
            Product product =
                UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();
            int expiredNum = 0;

            if (product != null)
            {
                if (product.ProductType.Name.ToLower() == "question")
                {
                    bool hasQuestion = UnitOfWork.OrderRepository.Get(current => current.UserId == userId &&
                    current.OrderType.Name == "question" && current.ExpireNumber > 0 && current.IsPaid == true).Any();
                    if (hasQuestion)
                    {
                        return "Invalid";
                    }
                }

                if (!string.IsNullOrEmpty(product.ExpireNumber.ToString()))
                    expiredNum = product.ExpireNumber.Value;


                decimal amount = product.Amount;

                if (product.IsInPromotion && product.DiscountAmount != null)
                    amount = product.DiscountAmount.Value;



                Order order = new Order();
                order.UserId = userId;
                order.Amount = amount;
                order.Code = FindeLastOrderCode() + 1;
                order.IsActive = true;
                order.ExpireNumber = expiredNum;
                order.IsPaid = false;
                order.OrderTypeId = product.ProductTypeId;
                order.TotalAmount = amount;
                order.DiscountAmount = 0;
                order.Email = email;

                if(product.ProductType.Name.ToLower() == "physicalproduct")
                {
                    order.CityId = cityId;
                    order.Address = address;
                    order.PostalCode = postalCode;
                }
               
                UnitOfWork.OrderRepository.Insert(order);

                foreach (var item in fullName)
                {
                    OrderDetail orderDetail = new OrderDetail();

                    orderDetail.ProductId = product.Id;
                    orderDetail.Fullname = item;
                    orderDetail.Quantity = 1;
                    orderDetail.Amount = amount;
                    orderDetail.RawAmount = amount;
                    orderDetail.OrderId = order.Id;
                    orderDetail.IsActive = true;

                   

                    UnitOfWork.OrderDetailRepository.Insert(orderDetail);
                    UnitOfWork.Save();
                }
                if (discountId != null)
                {

                    OrderDiscount orderDiscount = new OrderDiscount()
                    {
                        IsActive = true,
                        IsUse = true,
                        OrderId = order.Id,
                        DiscountCodeId = discountId.Value,
                        UsingDate = DateTime.Now,

                    };
                    order.DiscountAmount = discountAmout;
                    order.TotalAmount = amount - discountAmout.Value;

                    UnitOfWork.OrderDiscountRepository.Insert(orderDiscount);
                    UnitOfWork.OrderRepository.Update(order);
                    UnitOfWork.Save();
                }
                else
                    order.TotalAmount = order.Amount;


                

                return paymentUrl + "amount=" + order.TotalAmount.ToString().Split('/')[0] + "&orderid=" + order.Id;
            }
            return "false";

           
        }


        public int FindeLastOrderCode()
        {
            Order order = UnitOfWork.OrderRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();
            if (order != null)
                return order.Code;
            else
                return 999;
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