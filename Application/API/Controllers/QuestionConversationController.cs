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
    public class QuestionConversationController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly StatusManagement status = new StatusManagement();

        [Route("QuestionConversation/Post")]
        [HttpPost]
        [Authorize]
        public SuccessPostViewModel PostQuestionConversation(QuestionConversationInputViewModel input)
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
                string questionPostResult = GetQuestionPostResult(input.Subject, input.Body, user.Id);
                if (questionPostResult == "true")
                {
                    result.Result = Resources.Messages.SuccessPost;
                    result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);
                }
                else if(questionPostResult== "invalid")
                {
                    result.Result = Resources.Messages.InvalidPackage;
                    result.Status = status.ReturnStatus(0, Resources.Messages.InvalidPackage, false);
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
                var err = new HttpRequestMessage().CreateErrorResponse(HttpStatusCode.InternalServerError,
                    e.ToString());

                result.Result = null;
                result.Status = status.ReturnStatus(100, err.Content.ToString(), false);
                return result;
            }
        }



        [Route("QuestionConversation/List")]
        [HttpPost]
        [Authorize]
        public QuestionConversationListViewModel PostQuestionConversationList()
        {
            QuestionConversationListViewModel result = new QuestionConversationListViewModel();
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

                result.Result = GetQuestionList(user.Id);
                result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);

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


        [Route("QuestionConversation/Detail")]
        [HttpPost]
        [Authorize]
        public QuestionConversationDetailViewModel PostQuestionConversationDetail(QuestionConversationDetailInputViewModel input)
        {
            QuestionConversationDetailViewModel result = new QuestionConversationDetailViewModel();
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

                result.Result = GetQuestionConversationDetail(input.QuestionId);
                result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);

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

        #region Methods   

        public QuestionConversationDetailMasterViewModel GetQuestionConversationDetail(string questionId)
        {
            QuestionConversationDetailMasterViewModel detail = new QuestionConversationDetailMasterViewModel();
            Guid id = new Guid(questionId);
            QuestionConversation question = UnitOfWork.QuestionConversationRepository.GetById(id);

            if (question != null)
            {
                detail.Subject = question.Subject;
                detail.Status = GetQuestionStatuses(question.StatusCode);
                detail.Body = question.Body;
                detail.Conversations = GetConversation(id);
            }

            return detail;
        }

        public List<QuestionConversationDetailItemViewModel> GetConversation(Guid parentId)
        {
            List<QuestionConversationDetailItemViewModel> conversations =
                new List<QuestionConversationDetailItemViewModel>();

            List<QuestionConversation> questionConversations = UnitOfWork.QuestionConversationRepository
                .Get(current => current.ParentId == parentId).OrderBy(current => current.Order).ToList();

            foreach (QuestionConversation conversation in questionConversations)
            {
                conversations.Add(new QuestionConversationDetailItemViewModel()
                {
                    Body = conversation.Body,
                    CreationDate = conversation.CreationDateStr,
                    Order = conversation.Order.ToString(),
                    FullName = conversation.User.FullName
                });
            }

            return conversations;
        }
        public List<QuestionConversationItemViewModel> GetQuestionList(Guid userId)
        {
            List<QuestionConversationItemViewModel> list = new List<QuestionConversationItemViewModel>();

            List<QuestionConversation> questions = UnitOfWork.QuestionConversationRepository
                .Get(current => current.UserId == userId).ToList();

            foreach (QuestionConversation question in questions)
            {
                list.Add(new QuestionConversationItemViewModel()
                {
                    Subject = question.Subject,
                    CreationDate = question.CreationDateStr,
                    Status = GetQuestionStatuses(question.StatusCode),
                    Id = question.Id.ToString()
                });
            }

            return list;
        }

        public string GetQuestionStatuses(int status)
        {
            switch (status)
            {
                case 0:
                    return "در انتظار پاسخ";
                case 1:
                    return "پاسخ داده شد";
                default:
                    return "در دست بررسی";
            }
        }
        public string GetQuestionPostResult(string subject, string body, Guid userId)
        {
            if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(body))
                return "false";

            Order order = UnitOfWork.OrderRepository.Get(current => current.UserId == userId &&
                   current.OrderType.Name == "question" && current.ExpireNumber > 0 && current.IsPaid==true).FirstOrDefault();


            if (order != null)
            {
                QuestionConversation question = new QuestionConversation()
                {
                    UserId = userId,
                    Subject = subject,
                    Body = body,
                    ParentId = null,
                    Order = 1,
                    StatusCode = 0,
                    IsActive = true,
                };

                order.ExpireNumber = order.ExpireNumber - 1;


                UnitOfWork.QuestionConversationRepository.Insert(question);
                UnitOfWork.Save();


                return "true";
            }
            else
                return "invalid";
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