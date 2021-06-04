using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Models;
using ViewModels;
using System.IO;
using System.Drawing;
using System.Data.Entity;
using Helper;

namespace Payment.Controller
{
    public class HomeController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [Route("paymentrequest")]

        public ActionResult Index(decimal amount, Guid orderId)
        {
            string val = "false";

            if (Convert.ToDecimal(amount) == 0)
                return RedirectToAction("CallBackFree", new { orderId = orderId });

            else
            {
                val = ZarinPalRedirect(amount, orderId);


            }

            if (val == "false")
                return View();
            else
                return Redirect(val);
        }

        private String MerchantId = "3f5c16a0-7fe8-11e9-a12a-000c29344814";


        public string ZarinPalRedirect(decimal amount, Guid orderId)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

            String CallbackURL = WebConfigurationManager.AppSettings["callBackUrl"];

            long Amount = Convert.ToInt64(amount);



            String description = "خرید از اپلیکیشن قانون گستر";

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantId, Amount, CallbackURL, description);

            zarinpal.DisableSandboxMode();
            try
            {
                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    InsertToAuthority(orderId, res.Authority, amount);

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

        [Route("callback")]
        public ActionResult CallBack(string authority, string status)
        {
            String Status = status;
            CallBackViewModel callBack = new CallBackViewModel();

            if (Status != "OK")
            {
                callBack.IsSuccess = false;
            }

            else
            {
                try
                {
                    var zarinpal = ZarinPal.ZarinPal.Get();
                    zarinpal.DisableSandboxMode();
                    String Authority = authority;
                    long Amount = GetAmountByAuthority(Authority);

                    var verificationRequest = new ZarinPal.PaymentVerification(MerchantId, Amount, Authority);
                    var verificationResponse = zarinpal.InvokePaymentVerification(verificationRequest);
                    if (verificationResponse.Status == 100)
                    {
                        Order order = GetOrderByAuthority(authority);
                        if (order != null)
                        {
                            order.IsPaid = true;
                            order.PaymentDate = DateTime.Now;
                            order.RefId = verificationResponse.RefID;

                            UnitOfWork.OrderRepository.Update(order);
                            UnitOfWork.Save();

                            callBack.IsSuccess = true;
                            callBack.OrderCode = order.Code.ToString();
                            callBack.RefrenceId = verificationResponse.RefID;

                            // UpdateUserPoint(order);

                            string orderType = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId).Name;
                            callBack.ProductType = orderType;

                            if (!string.IsNullOrEmpty(orderType))
                            {
                                if (orderType.ToLower() == "vippackage")
                                    CheckVipPackage(order);
                                if (orderType.ToLower() == "workshop" || orderType.ToLower() == "event")
                                {
                                    //   GeneratePfd(order.Id);
                                }


                                OrderDetail orderDetail = UnitOfWork.OrderDetailRepository
                                    .Get(current => current.OrderId == order.Id).FirstOrDefault();

                                if (orderDetail != null)
                                {
                                    Product product = UnitOfWork.ProductRepository.GetById(orderDetail.ProductId);

                                    if (product != null)
                                    {
                                        string fileLink = "https://ghanongostar.zavoshsoftware.com/" + product.FileUrl;
                                        callBack.DownloadLink = fileLink;

                                        ViewBag.Email = order.Email;

                                        CreateEmail(order.Email, fileLink, orderType);

                                        CreateEmailForAdmin(product.Title, Amount);

                                        User user = UnitOfWork.UserRepository.GetById(order.UserId);
                                        CreateEmailForAdminForPhysicalProduct(product.Title, user.CellNum, order.Address,
                                            "", "", user.FullName);
                                    }
                                  

                                }

                            }
                        }
                        else
                        {
                            callBack.IsSuccess = false;
                            callBack.RefrenceId = "سفارش پیدا نشد";
                        }
                    }
                    else
                    {
                        callBack.IsSuccess = false;
                        callBack.RefrenceId = verificationResponse.Status.ToString();
                    }
                }
                catch (Exception e)
                {
                    callBack.IsSuccess = false;
                    callBack.RefrenceId = "خطا سیستمی. لطفا با پشتیبانی سایت تماس بگیرید";
                }
            }
            return View(callBack);

        }


        [Route("freecallback")]
        public ActionResult CallBackFree(Guid orderId)
        {
            CallBackViewModel callBack = new CallBackViewModel();

            try
            {

                Order order = UnitOfWork.OrderRepository.GetById(orderId);
                if (order != null)
                {
                    order.IsPaid = true;
                    order.PaymentDate = DateTime.Now;

                    UnitOfWork.OrderRepository.Update(order);
                    UnitOfWork.Save();

                    callBack.IsSuccess = true;
                    callBack.OrderCode = order.Code.ToString();
                    // callBack.RefrenceId = verificationResponse.RefID;

                    // UpdateUserPoint(order);

                    string orderType = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId).Name;
                    callBack.ProductType = orderType;
                    if (!string.IsNullOrEmpty(orderType))
                    {
                        if (orderType.ToLower() == "vippackage")
                            CheckVipPackage(order);
                        if (orderType.ToLower() == "workshop" || orderType.ToLower() == "event")
                        {
                            //   GeneratePfd(order.Id);
                        }


                        OrderDetail orderDetail = UnitOfWork.OrderDetailRepository
                            .Get(current => current.OrderId == order.Id).FirstOrDefault();

                        if (orderDetail != null)
                        {
                            Product product = UnitOfWork.ProductRepository.GetById(orderDetail.ProductId);

                            if (product != null)
                            {
                                string fileLink = "https://ghanongostar.zavoshsoftware.com/" + product.FileUrl;
                                callBack.DownloadLink = fileLink;


                                ViewBag.Email = order.Email;
                                CreateEmail(order.Email, fileLink, orderType);
                            }
                        }

                    }
                }
                else
                {
                    callBack.IsSuccess = false;
                    callBack.RefrenceId = "سفارش پیدا نشد";
                }

            }
            catch (Exception e)
            {
                callBack.IsSuccess = false;
                callBack.RefrenceId = "خطا سیستمی. لطفا با پشتیبانی سایت تماس بگیرید";
            }

            return View(callBack);

        }
        public void CheckVipPackage(Order order)
        {


            order.User.IsVip = true;
            UnitOfWork.UserRepository.Update(order.User);

            VipPackage vipPackage = UnitOfWork.VipPackageRepository.Get(current => current.Product.ProductTypeId == order.OrderTypeId).FirstOrDefault();

            if (vipPackage != null)
            {
                UserVipPackage userVipPackage = new UserVipPackage()
                {
                    Id = Guid.NewGuid(),
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    UserId = order.UserId,
                    VipPackegeId = vipPackage.Id,
                    ExpiredDate = DateTime.Now.AddMonths(vipPackage.Duration)
                };

                UnitOfWork.UserVipPackageRepository.Insert(userVipPackage);
            }

            UnitOfWork.Save();

        }
        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                UnitOfWork.ZarinpallAuthorityRepository.Get(current => current.Authority == authority).FirstOrDefault();

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }


        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
              UnitOfWork.ZarinpallAuthorityRepository.Get(current => current.Authority == authority).FirstOrDefault();

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }

        public void GeneratePfd(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);
            if (order != null)
            {
                List<OrderDetail> orderDetails = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == id).ToList();
                foreach (OrderDetail detail in orderDetails)
                {
                    Guid newId = Guid.NewGuid();
                    string path = Path.Combine(Server.MapPath("/Uploads/Order/"), Path.GetFileName(newId + ".pdf"));
                    //if (System.IO.File.Exists(path))
                    //{
                    //    Directory.Delete(path);
                    //}


                    var pdf = new Rotativa.ActionAsPdf("PrintDetails", new { id = detail.Id }) { FileName = newId + ".pdf", SaveOnServerPath = path };


                    var binary = pdf.BuildFile(this.ControllerContext);
                    detail.OrderFile = "/Uploads/Order/" + newId + ".pdf";
                    UnitOfWork.OrderDetailRepository.Update(detail);
                    UnitOfWork.Save();
                    //return pdf;
                }
            }
            //return null;

        }

        public ActionResult PrintDetails(Guid id)
        {
            OrderDetail orderDetail = UnitOfWork.OrderDetailRepository.GetById(id);
            OrderDetailViewModel orderDetailViewModel = new OrderDetailViewModel();

            orderDetailViewModel.Fullname = orderDetail.Fullname;
            orderDetailViewModel.Title = UnitOfWork.ProductRepository.GetById(orderDetail.ProductId).Title;

            Order order = UnitOfWork.OrderRepository.GetById(orderDetail.OrderId);
            orderDetailViewModel.Type = UnitOfWork.ProductTypeRepository.GetById(order.OrderTypeId).Title;



            return View(orderDetailViewModel);
        }
        public void UpdateUserPoint(Order order)
        {
            OrderDetail detail = UnitOfWork.OrderDetailRepository.Get(current => current.OrderId == order.Id).Include(current => current.Product).FirstOrDefault();
            int point = detail.Product.Point;
            order.User.Rate = point + order.User.Rate;
            //order.User.Rate = int.Parse(point.ToString());

            UnitOfWork.UserRepository.Update(order.User);
            UnitOfWork.Save();

        }

        public void CreateEmail(string email, string file, string productType)
        {
            Helper.Message message = new Message();
            string body = GetMessageBody(file, productType);

            message.Send(email, "خرید از اپلیکیشن قانون گستر", body, "email");
        }

        public void SendEmailNow()
        {
            CreateEmail("babaei.aho@gmail.com", null, "forms");
        }

        public string GetMessageBody(string file, string productType)
        {
            string body;
            if (productType == "forms")
                body = @"<html>
                 <head></head>
                <body dir='rtl'>
                <h1> خرید از اپلیکیشن قانون گستر</h1>
                <p> با تشکر از خرید شما از اپلیکیشن قانون گستر</p>
                <p> لینک دانلود اپلیکیشن<a href= '" + file + @"' > دانلود</a></p>
                <h2>دانلود اپلیکیشن قانون گستر</h2>
                <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
                <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
                <h3>مجموعه قانون گستر</h3>
                <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
                <p>تلفن: 02177515152</p>
                <p>وب سایت: https://ghanongostar.com/ </p>
                </body>
                </html> ";

            else if (productType == "physicalproduct" || productType == "event" || productType == "workshop")
            {
                body = @"<html>
                             <head></head>
                <body dir='rtl'>
                <h1> خرید از اپلیکیشن قانون گستر</h1>
                <p> با تشکر از خرید شما از اپلیکیشن قانون گستر</p>
                <p>همکاران ما جهت هماهنگی با شما تماس خواهند گرفت</p>
                <h2>دانلود اپلیکیشن قانون گستر</h2>
                <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
                <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
                <h3>مجموعه قانون گستر</h3>
                <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
                <p>تلفن: 02177515152</p>
                <p>وب سایت: https://ghanongostar.com/ </p>
                </body>
                </html> ";
            }

            else
            {
                body = @"<html>
                             <head></head>
                <body dir='rtl'>
                <h1> خرید از اپلیکیشن قانون گستر</h1>
                <p> با تشکر از خرید شما از اپلیکیشن قانون گستر</p>
                <h2>دانلود اپلیکیشن قانون گستر</h2>
                <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
                <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
                <h3>مجموعه قانون گستر</h3>
                <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
                <p>تلفن: 02177515152</p>
                <p>وب سایت: https://ghanongostar.com/ </p>
                </body>
                </html>";
            }

            return body;
        }
        public void CreateEmailForAdminForPhysicalProduct(string productTitle, string cellNumber, string address, string city, string postalCode, string fullName)
        {
            Helper.Message message = new Message();

            string email = "akramfazli2011@gmail.com";
            // string email = "babaei.aho@gmail.com";
            string body = @"<html>
                 <head></head>
                <body dir='rtl'>
                <h1> خرید محصول از اپلیکیشن قانون گستر</h1>
                <p> از اپلیکیشن قانون گستر خریدی انجام شده است</p>
                <p>عنوان کالا: </p>" + productTitle + "<p>شماره موبایل کاربر: </p>" + cellNumber +
                          "<p>نام کاربر: </p>" + fullName + "<p>شهر: </p>" + city +
                          "<p>آدرس: </p>" + address + @"
                <h3>مجموعه قانون گستر</h3>
                <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
                <p>تلفن: 02177515152</p>
                <p>وب سایت: https://ghanongostar.com/ </p>
                </body>
                </html> ";

            message.Send(email, "خرید از اپلیکیشن قانون گستر", body, "email");
        }


        public void CreateEmailForAdmin(string productTitle, long amount)
        {
            Helper.Message message = new Message();

            string email = "hajizadehlaw@yahoo.com";
            string body = @"<html>
                 <head></head>
                <body dir='rtl'>
                <h1> خرید از اپلیکیشن قانون گستر</h1>
                <p> از اپلیکیشن قانون گستر خریدی انجام شده است</p>
                <p>عنوان کالا: </p>" + productTitle + @"
                <p>مبلغ پرداخت شده: </p>" + amount + @"
                <h2>دانلود اپلیکیشن قانون گستر</h2>
                <p><a href='https://play.google.com/store/apps/details?id=com.zavosh.software.ghanongostar.company&hl=en'>دانلود نسخه اندروید از گوگل پلی</a></p>
                <p><a href='https://sibche.com/applications/ghanon-gostar'>دانلود نسخه ios از سیبچه</a></p>
                <h3>مجموعه قانون گستر</h3>
                <p>آدرس: تهران، خیابان شریعتی، سه راه طالقانی، جنب مبل پایتخت، پلاک 306، طبقه 2، واحد 4</p>
                <p>تلفن: 02177515152</p>
                <p>وب سایت: https://ghanongostar.com/ </p>
                </body>
                </html> ";

            message.Send(email, "خرید از اپلیکیشن قانون گستر", body, "email");
        }

    }
}