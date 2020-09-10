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

namespace API.Controllers
{
    public class DiscountController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        [Route("discount")]
        [HttpPost]
        [Authorize]
        public DiscountViewModel PostDiscountCode(DiscountInputViewModel input)
        {
            DiscountViewModel result = new DiscountViewModel();
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

                result = CheckDiscountCode(input.Code, input.Amount, user);


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

        public DiscountViewModel CheckDiscountCode(string code, string amount, User user)
        {
            DiscountViewModel result = new DiscountViewModel();
            DiscountCode discountCode = UnitOfWork.DiscountCodeRepository.Get(current => current.Code == code && current.IsDeleted == false && current.IsActive == true).FirstOrDefault();
            if (discountCode != null)
            {
                if (discountCode.ExpireDate <= DateTime.Now)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(100, Resources.Messages.Expired_DiscountCode, false);
                    return result;
                }

                if (discountCode.IsMultiUsing == false)
                {
                    int orderDiscount = UnitOfWork.OrderDiscountRepository.Get(current => current.DiscountCodeId == discountCode.Id && current.Order.UserId == user.Id).Count();
                    if (orderDiscount > 1)
                    {
                        result.Result = null;
                        result.Status = status.ReturnStatus(100, Resources.Messages.Invalid_DiscountCode, false);
                        return result;
                    }

                }

                ProductDiscount productDiscount = UnitOfWork.ProductDiscountRepository.Get(current =>
                        current.IsDeleted == false && current.IsActive && current.DiscountCodeId == discountCode.Id)
                    .FirstOrDefault();

                
                if (productDiscount != null)
                {
                    if (amount == "80000" || amount == "40000"||amount=="300000"||amount=="690000"||amount=="490000")
                    {
                    }
                    else
                    {
                        result.Result = null;
                        result.Status = status.ReturnStatus(100, "کد تخفیف وارد شده برای محصول مورد نظر معتبر نمی باشد",
                            false);
                        return result;
                    }
                }


                if (discountCode.IsPercent)
                {
                    DiscountResult discountResult = new DiscountResult();
                    discountResult.DiscountAmount =
                        ((discountCode.Amount * Convert.ToDecimal(amount)) / 100).ToString();
                    discountResult.DiscountId = discountCode.Id;
                    result.Result = discountResult;
                    result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);
                }
                else
                {
                    DiscountResult discountResult = new DiscountResult();
                    discountResult.DiscountAmount = discountCode.Amount.ToString();
                    discountResult.DiscountId = discountCode.Id;
                    result.Result = discountResult;
                    result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);
                }
            }

            else
            {
                result.Result = null;
                result.Status = status.ReturnStatus(100, Resources.Messages.Invalid_DiscountCode, false);
            }
            return result;
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