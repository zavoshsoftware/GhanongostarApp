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
    public class CourseDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        [Route("CourseDetails/List")]
        [HttpPost]
        [Authorize]
        public CourseDetailListViewModel PostCourseDetailList(ProductDetailInputViewModel input)
        {
            CourseDetailListViewModel result = new CourseDetailListViewModel();
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

                result.Result = GetCourseDetailList(input.ProductCode, user.Id);
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



        #region List   

        public List<CourseDetailItemViewModel> GetCourseDetailList(string productCode, Guid userId)
        {
            List<CourseDetailItemViewModel> list = new List<CourseDetailItemViewModel>();

            int code = Convert.ToInt32(productCode);

            Product product = UnitOfWork.ProductRepository.Get(current => current.Code == code).FirstOrDefault();

            if (product != null)
            {
                string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

                List<CourseDetail> courseDetails = UnitOfWork.CourseDetailRepository
                    .Get(current => current.ProductId == product.Id && current.IsActive).OrderBy(current => current.SessionNumber).ToList();

                foreach (CourseDetail courseDetail in courseDetails)
                {
                    list.Add(new CourseDetailItemViewModel()
                    {
                        Title = courseDetail.Title,
                        VideoUrl = baseUrlPanel + courseDetail.VideoUrl,
                        SessionNumber = courseDetail.SessionNumber.ToString(),
                        Summery = courseDetail.Summery,
                        ThumbnailImageUrl = baseUrlPanel + courseDetail.ThumbnailImageUrl
                    });
                }
            }

            return list;
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