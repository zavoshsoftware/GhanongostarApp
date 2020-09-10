using Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Helpers;
using ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SmsIrRestful;

namespace Site.Controllers
{
    public class AccountController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();
        BaseViewModelHelper baseViewModel = new BaseViewModelHelper();

        [Route("login")]
        public ActionResult Login(string ReturnUrl = "")
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;

            LoginViewModel login = new LoginViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenuProductGroups()
            };

            return View(login);
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                User oUser = db.Users.Include(u => u.Role)
                    .FirstOrDefault(a =>
                    a.CellNum == model.Username
                    && a.Password == model.Password
                    && a.IsActive
                    && a.IsDeleted == false);

                if (oUser != null)
                {
                    var ident = new ClaimsIdentity(
                      new[] { 
              // adding following 2 claim just for supporting default antiforgery provider
              new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
              new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

              new Claim(ClaimTypes.Name,oUser.Id.ToString()),

              // optionally you could add roles if any
               new Claim(ClaimTypes.Role, oUser.Role.Name),
               new Claim(ClaimTypes.Surname, oUser.FullName),

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                    HttpContext.GetOwinContext().Authentication.SignIn(
                       new AuthenticationProperties
                       {
                           IsPersistent = true,
                           ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(600),

                       },
                       ident);
                    return RedirectToLocal(returnUrl, oUser.Role.Name); // auth succeed 
                }
                else
                {
                    // invalid username or password
                    TempData["WrongPass"] = "شماره موبایل و یا کلمه عبور وارد شده صحیح نمی باشد.";
                }
            }
            // If we got this far, something failed, redisplay form
            LoginViewModel login = new LoginViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenuProductGroups(),
                Username = model.Username,
                Password = model.Password
            };

            return View(login);
        }


        [Route("activate")]
        public ActionResult Activate(int userCode)
        {
            User user = db.Users.FirstOrDefault(current => current.Code == userCode);

            if (user != null)
                ViewBag.cellNumber = user.CellNum;

            ActivateViewModel activate = new ActivateViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenuProductGroups()
            };
            ViewBag.UserCode = userCode;
            return View(activate);

        }

        [Route("activate")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Activate(ActivateViewModel activate, int userCode)
        {
            User user = db.Users.FirstOrDefault(current => current.Code == userCode);
            if (user != null)
            {
                ActivationCode activationCode =
                    db.ActivationCodes.FirstOrDefault(current =>
                        current.UserId == user.Id && current.Code == activate.Code);

                if (activationCode != null)
                {
                    ActivateUser(user);
                    UpdateActivationCode(activationCode);
                    db.SaveChanges();

                    return Redirect("/basket");
                }
                else
                {

                    TempData["WrongCode"] = "کد وارد شده صحیح نمی باشد";
                }
            }
            else
            {
                TempData["WrongCode"] = "کد وارد شده صحیح نمی باشد";
            }

            activate.MenuProductGroups = baseViewModel.GetMenuProductGroups();
            return View(activate);

        }

        [Route("register")]
        public ActionResult Register(string ReturnUrl, string qty)
        {
            ViewBag.Message = "";
            ViewBag.ReturnUrl = ReturnUrl;

            RegisterViewModel register = new RegisterViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenuProductGroups()
            };
            return View(register);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Guid userRoleId = ReturnUserEmployeeRole();

                Guid userEmployerRoleId = ReturnUserEmployerRole();

                User oUser = db.Users.Include(u => u.Role).FirstOrDefault(a =>
                    a.CellNum == model.CellNumber && (a.RoleId == userRoleId || a.RoleId == userEmployerRoleId) &&
                    a.IsActive && a.IsDeleted == false);

                if (oUser == null)
                {
                    User user = CreateUserObject(model.FullName, model.CellNumber, userRoleId);

                    UnitOfWork.UserRepository.Insert(user);
                    UnitOfWork.Save();

                    int code = CreateActivationCode(user.Id);
                    UnitOfWork.Save();
                    /* await*/
                    SendSms(model.CellNumber, code);
                    return RedirectToAction("Activate", new { userCode = user.Code });


                }
                else
                {
                    // duplicate cellnumber
                    TempData["duplicateUser"] = "شما قبلا با این شماره موبایل ثبت نام کرده اید. جهت بازیابی رمز عبور بر روی فراموشی رمز عبور کلیک کنید.";
                }
            }
            RegisterViewModel register = new RegisterViewModel()
            {
                MenuProductGroups = baseViewModel.GetMenuProductGroups()
            };
            ViewBag.ReturnUrl = returnUrl;
            return View(register);
        }



        public User CreateUserObject(string fullName, string cellNumber, Guid roleId)
        {
            User user = new User();

            user.FullName = fullName;
            user.CellNum = cellNumber;
            user.RoleId = roleId;
            user.IsActive = false;
            user.CreationDate = DateTime.Now;
            user.IsDeleted = false;
            user.Code = ReturnCode();

            return user;
        }

        public int ReturnCode()
        {
            Guid userRoleId = ReturnUserEmployeeRole();
            Guid userEmployerRoleId = ReturnUserEmployerRole();
            User user = UnitOfWork.UserRepository.Get(current => current.RoleId == userRoleId || current.RoleId == userEmployerRoleId).OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 300001;
            }
        }
        public Guid ReturnUserEmployeeRole()
        {
            Guid roleId = new Guid("6D352C2F-6E64-4762-AAE4-00F49979D7F1");
            return roleId;
        }

        public Guid ReturnUserEmployerRole()
        {
            Guid roleId = new Guid("B999EB27-7330-4062-B81F-62B3D1935885");
            return roleId;
        }
        public int CreateActivationCode(Guid userId)
        {
            DeactiveOtherActivationCode(userId);

            int code = RandomCode();
            ActivationCode activationCode = new ActivationCode();
            activationCode.UserId = userId;
            activationCode.Code = code;
            activationCode.ExpireDate = DateTime.Now.AddDays(2);
            activationCode.IsUsed = false;
            activationCode.IsActive = true;
            activationCode.CreationDate = DateTime.Now;
            activationCode.IsDeleted = false;

            UnitOfWork.ActivationCodeRepository.Insert(activationCode);

            return code;
        }

        public void DeactiveOtherActivationCode(Guid userId)
        {
            List<ActivationCode> activationCodes = db.ActivationCodes
                .Where(current => current.UserId == userId && current.IsActive).ToList();

            foreach (ActivationCode activationCode in activationCodes)
            {
                activationCode.IsActive = false;
                activationCode.LastModifiedDate = DateTime.Now;
            }

        }

        private Random random = new Random();
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 10000).ToString("D5");
            return Convert.ToInt32(r);
        }

        public void SendSms(string cellNumber, int code)
        {
            var token = new Token().GetToken("877bf267654c01afa079f268", "it66)%#teBC!@*&");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 10382,
                ParameterArray = new List<UltraFastParameters>()
                {
                    new UltraFastParameters()
                    {
                        Parameter = "VerificationCode" , ParameterValue = code.ToString()
                    }
                }.ToArray()

            };

            UltraFastSendRespone ultraFastSendRespone = new UltraFast().Send(token, ultraFastSend);

            if (ultraFastSendRespone.IsSuccessful)
            {

            }
            else
            {

            }
        }


        private ActionResult RedirectToLocal(string returnUrl, string roleName)
        {

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                if (roleName == "Administrator")
                    return RedirectToAction("index", "Products");

                return RedirectToAction("Index", "home");
            }
        }
        public ActionResult LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut();
            }
            return Redirect("/");
        }


        #region ActivateHelper
        public void ActivateUser(User user)
        {

            user.IsActive = true;
            user.LastModifiedDate = DateTime.Now;
        }

        public void UpdateActivationCode(ActivationCode activationCode)
        {
            activationCode.IsUsed = true;
            activationCode.UsingDate = DateTime.Now;
            activationCode.IsActive = false;
            activationCode.LastModifiedDate = DateTime.Now;
        }

        #endregion


        //[Route("profile")]
        //[Authorize]
        //public ActionResult Profile()
        //{
        //    ProfileViewModel profile = new ProfileViewModel();
        //    profile.Brands = baseViewModel.GetMenu();
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
        //        string id = identity.FindFirst(System.Security.Claims.ClaimTypes.Name).Value;

        //        Guid userId = new Guid(id);

        //        User user = db.Users.Find(userId);

        //        if (user != null)
        //        {
        //            profile.User = user;
        //            profile.Orders = db.Orders.Where(current => current.UserId == userId)
        //                .OrderByDescending(c => c.CreationDate).ToList();
        //        }
        //    }
        //    return View(profile);
        //}





        [AllowAnonymous]
        public ActionResult SendOtp(string cellNumber)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
                bool isValidMobile = Regex.IsMatch(cellNumber, @"(^(09|9)[0-9][0-9]\d{7}$)|(^(09|9)[3][12456]\d{7}$)", RegexOptions.IgnoreCase);

                if (isValidMobile)
                {
                    User user = UnitOfWork.UserRepository.Get(current => current.CellNum == cellNumber).FirstOrDefault();

                    if (user != null)
                    {
                        int code = Convert.ToInt32(user.Password);

                        SendSms(cellNumber, code);

                        return Json("true", JsonRequestBehavior.AllowGet);
                    }

                    return Json("invalidUser", JsonRequestBehavior.AllowGet);

                }
                return Json("invalidCellNumber", JsonRequestBehavior.AllowGet);

                //else
                //{
                //    Guid userId = CreateUser(fullName, cellNumber, email, employeeType);
                //    int codeInt = CreateActivationCode(userId);
                //    code = codeInt.ToString();
                //}


                //UnitOfWork.Save();

            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        [AllowAnonymous]
        public ActionResult CompleteRegister(string cellNumber, string employeeType, string fullName)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == cellNumber).FirstOrDefault();

                int code = 0;

                if (user == null)
                {
                    Guid userId = CreateUser(fullName, cellNumber, employeeType);

                    int codeInt = CreateActivationCode(userId);

                    code = codeInt;

                    UnitOfWork.Save();
                    
                    SendSms(cellNumber, code);

                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                return Json("false", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        [AllowAnonymous]
        public ActionResult CheckOtp(string cellNumber, string activationCode)
        {
            try
            {
                cellNumber = cellNumber.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");
                activationCode = activationCode.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("v", "7").Replace("۸", "8").Replace("۹", "9");

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == cellNumber).FirstOrDefault();

                if (user != null)
                {
                    ActivationCode activation = IsValidActivationCode(user.Id, activationCode);

                    if (activation != null)
                    {
                        ActivateUser(user, activationCode);
                        UpdateActivationCode(activation, null, null, null, null);
                        UnitOfWork.Save();

                        LoginWithOtp(user);
                        return Json("true", JsonRequestBehavior.AllowGet);
                    }

                    if (user.IsActive && user.Password == activationCode)
                    {
                        LoginWithOtp(user);
                        return Json("true", JsonRequestBehavior.AllowGet);
                    }
                }
                return Json("invalid", JsonRequestBehavior.AllowGet);
            }

            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActivationCode IsValidActivationCode(Guid userId, string activationCode)
        {
            int code = Convert.ToInt32(activationCode);

            ActivationCode oActivationCode = UnitOfWork.ActivationCodeRepository
                .Get(current => current.UserId == userId && current.Code == code && current.IsUsed == false
                                && current.IsActive == true && current.ExpireDate > DateTime.Now).FirstOrDefault();

            if (oActivationCode != null)
                return oActivationCode;
            else
                return null;
        }

        public void ActivateUser(User user, string code)
        {
            user.IsActive = true;
            user.LastModifiedDate = DateTime.Now;
            user.Password = code;

            UnitOfWork.UserRepository.Update(user);
        }

        public void UpdateActivationCode(ActivationCode activationCode, string deviceId, string deviceModel,
            string OsType, string OsVersion)
        {
            activationCode.IsUsed = true;
            activationCode.UsingDate = DateTime.Now;
            activationCode.IsActive = false;
            activationCode.LastModifiedDate = DateTime.Now;
            activationCode.DeviceId = deviceId;
            activationCode.DeviceModel = deviceModel;
            activationCode.OsType = OsType;
            activationCode.OsVersion = OsVersion;

            UnitOfWork.ActivationCodeRepository.Update(activationCode);
        }


        public Guid CreateUser(string fullName, string cellNumber, string employeeType)
        {
            Guid roleId = new Guid("B999EB27-7330-4062-B81F-62B3D1935885");

            if (employeeType == "karmand")
                roleId = new Guid("6D352C2F-6E64-4762-AAE4-00F49979D7F1");

            User user = new User()
            {
                CellNum = cellNumber,
                FullName = fullName,
                RoleId = roleId,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                Code = ReturnCode(),
                IsActive = false,
                Id = Guid.NewGuid()
            };

            UnitOfWork.UserRepository.Insert(user);
            UnitOfWork.Save();
            return user.Id;
        }


        public void LoginWithOtp(User oUser)
        {
            var ident = new ClaimsIdentity(
                new[] { 
                    // adding following 2 claim just for supporting default antiforgery provider
                    new Claim(ClaimTypes.NameIdentifier, oUser.CellNum),
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                    new Claim(ClaimTypes.Name,oUser.Id.ToString()),

                    // optionally you could add roles if any
                    new Claim(ClaimTypes.Role, oUser.Role.Name),
                    new Claim(ClaimTypes.Surname, oUser.FullName),

                },
                DefaultAuthenticationTypes.ApplicationCookie);

            HttpContext.GetOwinContext().Authentication.SignIn(
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(600),

                },
                ident);
            
        }
    }
}