using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Models;

namespace Helpers
{
    public class ZarinPalHelper:Infrastructure.BaseControllerWithUnitOfWork
    {
        private String MerchantId = "3f5c16a0-7fe8-11e9-a12a-000c29344814";
        private DatabaseContext db = new DatabaseContext();

        public string GetMerchantId()
        {
            return MerchantId;
        }
        public string ZarinPalRedirect(Order order, decimal amount)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

            String CallbackURL = WebConfigurationManager.AppSettings["callBackUrl"];

            long Amount = Convert.ToInt64(amount);

            List<OrderDetail> orderDetails = db.OrderDetails.Where(current => current.OrderId == order.Id).Include(c=>c.Product).ToList();
            string productDesc = null;
            foreach (OrderDetail orderDetail in orderDetails)
            {
                productDesc = productDesc + orderDetail.Product.Code + ",";
            }

            String description = "خرید محصول کد" + productDesc;

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantId, Amount, CallbackURL, description);

            zarinpal.DisableSandboxMode();
            try
            {
                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    InsertToAuthority(order.Id, res.Authority, amount);

                    return res.PaymentURL;
                }
                else
                    return "false";


            }
            catch (Exception e)
            {
                return "zarrin";
            }
        }

        public void InsertToAuthority(Guid orderId, string authority, decimal amount)
        {
            ZarinpallAuthority zarinpallAuthority = new ZarinpallAuthority()
            {
                OrderId = orderId,
                Authority = authority,
                Amount = amount,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true
            };

            UnitOfWork.ZarinpallAuthorityRepository.Insert(zarinpallAuthority);
            UnitOfWork.Save();
        }

        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }

        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }
    }
}