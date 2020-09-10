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
    public class ProductGroupController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

            PageCounter pageCounter = new PageCounter();
        [Route("Productgroup/List")]
        [HttpPost]
        [Authorize]
        public ProductGroupLisViewModel PostProductList()
        {
            ProductGroupLisViewModel result = new ProductGroupLisViewModel();
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

                result.Result = GetProductGroupList();
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);
                pageCounter.Count("videogrouplist", null);

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
     
        public List<ProductGroupListItemViewModel> GetProductGroupList()
        {
            List<ProductGroupListItemViewModel> productGroupList = new List<ProductGroupListItemViewModel>();

            List<ProductGroup> productGroups =
                UnitOfWork.ProductGroupRepository.Get(current=>current.IsActive).ToList();

            string baseUrlPanel = WebConfigurationManager.AppSettings["BaseUrlPanel"];

            foreach (ProductGroup productGroup in productGroups)
            {
                productGroupList.Add(new ProductGroupListItemViewModel()
                {
                    Image = baseUrlPanel + productGroup.ImageUrl,
                    Title = productGroup.Title,
                    Code = productGroup.Code.ToString()
                });

            }
            return productGroupList;
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