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
    public class SupportRequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        PageCounter pageCounter = new PageCounter();
        [Route("SupportTypeRequest/List")]
        [HttpPost]
        [Authorize]
        public SupportRequestViewModel PostSupportType()
        {
            SupportRequestViewModel result = new SupportRequestViewModel();
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
                result.Result = RetunResult(user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.Success, true);
                pageCounter.Count("support",null);

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

        [Route("SupportRequest/Post")]
        [HttpPost]
        [Authorize]
        public SuccessPostViewModel PostSupportRequest(SupportRequestInputViewModel input)
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
                if (GetRequestResult(input.TypeId, input.Body,user.Id))
                {
                    result.Result = Resources.Messages.SuccessPost;
                    result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost,true);
                }
                else
                {
                    result.Result = Resources.Messages.Failed;
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

        [Route("SupportRequest/List")]
        [HttpPost]
        [Authorize]
        public SupportRequestListViewModel PostSupportRequestList()
        {
            SupportRequestListViewModel result = new SupportRequestListViewModel();
            try
            {
                string token = GetRequestHeader();
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    
                }
                else
                {
                    result.Result = ReturnRequestListResult(user.Id);
                    result.Status= status.ReturnStatus(0, Resources.Messages.Success, true);
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
        public SupportItems RetunResult(Guid id)
        {
            SupportItems items = new SupportItems();
            items.KeyValueList = ReturnKeyValueList();
            items.SupportRequestTypeList = RetrunSupportType();
            items.IsCustomer = CheckCustomer(id);

            return items;
        }
        public List<SupportType> RetrunSupportType()
        {
            List<SupportType> supportTypes = new List<SupportType>();
            List<SupportRequestType> typeList = UnitOfWork.SupportRequestTypeRepository.Get().ToList();
            foreach (SupportRequestType type in typeList)
            {
                supportTypes.Add(new SupportType { Id = type.Id, Title = type.Title });
            }
            return supportTypes;
        }
        public bool CheckCustomer(Guid id)
        {
            bool isCustomer = UnitOfWork.OrderRepository.Get(current => current.UserId == id).Any();

            return isCustomer;
        }
        public List<KeyValueViewModel> ReturnKeyValueList()
        {
            List<KeyValueViewModel> KeyValueList = new List<KeyValueViewModel>();

            KeyValueList.Add(new KeyValueViewModel { Key = "تلفن", Value = UnitOfWork.TextRepository.Get(current => current.Title == "phone").FirstOrDefault().Body } );
            KeyValueList.Add(new KeyValueViewModel { Key = "آدرس", Value = UnitOfWork.TextRepository.Get(current => current.Title == "address").FirstOrDefault().Body });

            return KeyValueList;
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
        public SqlBoolean GetRequestResult(Guid typeId,string body,Guid id)
        {
            try
            {
                SupportRequest supportRequest = new SupportRequest()
                {
                    SupportRequestTypeId=typeId,
                    Body=body,
                    UserId=id,
                    IsActive=true,
                    Status=0,
                    Code= FindLastSupportCode()+1


                };
                UnitOfWork.SupportRequestRepository.Insert(supportRequest);
                UnitOfWork.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<SupportListItem> ReturnRequestListResult(Guid id)
        {
            List<SupportListItem> supportListItems = new List<SupportListItem>();
            List<SupportRequest> supportRequests = UnitOfWork.SupportRequestRepository.Get(current => current.UserId == id).ToList();
            foreach(SupportRequest request in supportRequests)
            {
                supportListItems.Add(new SupportListItem
                {
                    Body = request.Body,
                    Code = request.Code,
                    Response = request.Response,
                    Status = request.Status,
                    SubmitDate = request.CreationDateStr,
                    Title = request.Type.Title
                });
            }
            return supportListItems;
        }

        public int FindLastSupportCode()
        {
            SupportRequest request = UnitOfWork.SupportRequestRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();
            if (request != null)
                return request.Code;
            else
                return 999;
        }
        #endregion
    }
}