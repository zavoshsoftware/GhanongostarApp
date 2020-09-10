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
    public class BlogController : Infrastructure.BaseControllerWithUnitOfWork
    {
        PageCounter pageCounter = new PageCounter();
        readonly StatusManagement status = new StatusManagement();

        [Route("Blog/List")]
        [HttpPost]
        [Authorize]
        public BlogListViewModel PostBlogList()
        {
            BlogListViewModel result = new BlogListViewModel();
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

                result.Result = GetBlogList();
                result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);
                pageCounter.Count("newsgrouplist", null);


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

        public List<BlogItemViewModel> GetBlogList()
        {
            List<BlogItemViewModel> res = new List<BlogItemViewModel>();

            List<Blog> blogs = UnitOfWork.BlogRepository.Get(current => current.IsActive).ToList();
            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

            foreach (Blog blog in blogs)
            {
                res.Add(new BlogItemViewModel()
                {
                    Title = blog.Title,
                    ImageUrl = baseUrlPanel + blog.ImageUrl,
                    image = baseUrlPanel + blog.ImageUrl,
                    BlogCategoryTitle =  blog.BlogCategory.Title,
                    WebViewUrl = baseUrlPanel + "Blog/detail/" + blog.Id,

                });
            }

            return res;
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