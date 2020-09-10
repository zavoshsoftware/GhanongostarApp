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
    public class UserController : Infrastructure.BaseControllerWithUnitOfWork
    {
        StatusManagement status = new StatusManagement();


        [Authorize]
        [Route("user/get")]
        [HttpPost]
        public ProfileGetViewModel Get()
        {
            ProfileGetViewModel res = new ProfileGetViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    res.Result = null;
                    res.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return res;
                }

                res = GetProfileResult(user);
            }
            catch (Exception e)
            {
                res = ReturnReject();
            }
            return res;
        }

        [Authorize]
        [Route("user/Post")]
        [HttpPost]
        public SuccessPostViewModel Post(UserPostInputViewModel input)
        {
            SuccessPostViewModel result = new SuccessPostViewModel();
            string fullname = input.Fullname;
            string email = input.Email;
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
                if(PostProfileResult(user, fullname, email))
                {
                    result.Result = Resources.Messages.SuccessPost;
                    result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);
                }
                else
                {
                    result.Result = Resources.Messages.InputRequired;
                    result.Status = status.ReturnStatus(0, Resources.Messages.InputRequired, true);
                }
                return result;

            }
            catch (Exception e)
            {
                result.Result = null;
                result.Status = status.ReturnStatus(100, "خطا در بازیابی اطلاعات", false);
            }
            return result;
        }


        public bool PostProfileResult(User user, string fullname, string email)
        {
            if (string.IsNullOrEmpty(fullname) || string.IsNullOrEmpty(email))
                return false;

            user.FullName = fullname;
            user.Email = email;

            UnitOfWork.Save();
            return true;
        }

        public ProfileGetViewModel GetProfileResult(User user)
        {
            ProfileGetViewModel profile = new ProfileGetViewModel();
            profile.Result = GetProfileItems(user);
            profile.Status = status.ReturnStatus(0, "اطلاعات صفحه پروفایل اپلیکیشن", true);

            return profile;
        }

        public profileGetItemViewModel GetProfileItems(User user)
        {

            profileGetItemViewModel profile = new profileGetItemViewModel()
            {
                CellNumber = user.CellNum,
                FullName = user.FullName,
                Email = user.Email,
                Rate = user.Rate.ToString(),
                InviteText = user.FullName + " شما را برای دانلود و ثبت نام در اپلیکیشن قانونگستر دعوت کرده است. جهت دانلود اپلیکیشن بر روی لینک زیر کلیک کنید."
            };
            return profile;
        }

        public List<string> GetHeaderImages()
        {
            string baseUrl = WebConfigurationManager.AppSettings["BaseUrl"];

            List<string> images = new List<string>();
            for (int i = 1; i < 11; i++)
            {
                string fileName = "Images/Home/" + i + ".png";
                images.Add(baseUrl + fileName);
            }

            return images;
        }
        public ProfileGetViewModel ReturnReject()
        {
            ProfileGetViewModel homeViewModel = new ProfileGetViewModel();
            homeViewModel.Result = null;
            homeViewModel.Status = status.ReturnStatus(100, "خطا در بازیابی اطلاعات", false);

            return homeViewModel;
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
