using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using API.Models;
using Models;
using Services;
using Newtonsoft.Json.Linq;
using Helper;
using Models.Input;
using Viewmodels;
using SmsIrRestful;

namespace API.Controllers
{
    public class AccountController : Infrastructure.BaseControllerWithUnitOfWork
    {
        [Route("account/usercode")]
        public string GetUserCode()
        {
            List<User> users = UnitOfWork.UserRepository.Get().OrderBy(current => current.CreationDate).ToList();
            int code = 300001;
            foreach (var user in users)
            {
                user.Code = code;
                code++;
                UnitOfWork.UserRepository.Update(user);
            }
            UnitOfWork.Save();
            return "ok";
        }

        StatusManagement status = new StatusManagement();
        #region API
        [Route("account/register")]
        [HttpPost]
        public RegisterViewModel PostRegister(RegisterInputViewModel register)
        {
            string fullName = register.FullName;
            string cellNumber = register.CellNumber;
            Guid roleId = GetRoleId(register.IsEmployee);


            User existUser = IsUserExist(cellNumber);

            if (existUser == null)
            {
                User user = CreateUserObject(fullName, cellNumber, roleId);
                UnitOfWork.UserRepository.Insert(user);
                UnitOfWork.Save();

                int code = CreateActivationCode(user.Id);
                UnitOfWork.Save();
                /* await*/
                 SendSms(cellNumber, code);
                return CompleteJson(user, true, Resources.Messages.Register_Success, null, 0);
            }
            else if (existUser.IsActive == false)
            {
                existUser.FullName = fullName;

                UnitOfWork.UserRepository.Update(existUser);

                int code = CreateActivationCode(existUser.Id);
                UnitOfWork.Save();
                /* await*/
                 SendSms(cellNumber, code);

                return CompleteJson(existUser, true, Resources.Messages.Register_Success, null, 0);
            }
            else
                return CompleteJson(existUser, false, null, Resources.Messages.Register_DuplicateCellNumber, 3);
        }

        public Guid GetRoleId(string isEmployee)
        {
            if (isEmployee == "true")
                return new Guid("6d352c2f-6e64-4762-aae4-00f49979d7f1");
            else
                return new Guid("b999eb27-7330-4062-b81f-62b3d1935885");
        }

        [Route("account/DeleteAccount")]
        [HttpPost]
        public RegisterViewModel DeleteRAccount(DeleteAccountInputViewModel input)
        {
            string cellNumber = input.CellNumber;
            User existUser = IsUserExist(cellNumber);
            if (existUser != null)
            {
                UnitOfWork.UserRepository.Delete(existUser);
                UnitOfWork.Save();
                return CompleteJson(existUser, false, null, Resources.Messages.Success_DeleteUser, 3);
            }
            else
            {
                return CompleteJson(existUser, false, null, Resources.Messages.InvalidUser, 3);
            }

        }
 
        [Route("account/activate")]
        [HttpPost]
        public ActivateViewModel PostActivate(ActivateInputViewModel input)
        {
            string tokenId = input.TokenId;
            string activationCode = input.ActivationCode;
            string deviceId = input.DeviceId;
            string deviceModel = input.DeviceModel;
            string osType = input.OsType;
            string osVersion = input.OsVersion;


            ActivateViewModel activate = new ActivateViewModel();
            Status status = new Status();
            ActivateResult result = new ActivateResult();

            ActivationCode activation = IsValidActivationCode(tokenId, activationCode);
            if (activation != null)
            {
                User user = ActivateUser(tokenId, activationCode);
                UpdateActivationCode(activation, deviceId, deviceModel, osType, osVersion);
                UnitOfWork.Save();


                status.IsSuccess = true;
                status.Message = Resources.Messages.Register_SuccessActivate;
                status.StatusCode = 0;


                LoginResultViewModel login = Login(user.CellNum, user.Password);
                if (login.IsSuccess)
                {
                    UpdateUserWithToken(user, login.Token);
                    result.TokenId = login.Token;
                    UnitOfWork.Save();

                    SendActivationSuccess(user.CellNum, user.FullName);
                }
            }
            else
            {
                status.IsSuccess = false;
                status.Message = Resources.Messages.Registe_WrongActivationCode;
                status.StatusCode = 2;
            }

            activate.Result = result;
            activate.Status = status;
            return activate;
        }

        [Route("account/resendcode")]
        [HttpPost]
        public RegisterViewModel PostResendActivationCode(ResendCodeInputViewModel input)
        {
            string tokenId = input.TokenId;
            Guid userId = new Guid(tokenId);
            User user = IsValidToken(userId);
            if (user != null)
            {
                if (CheckSendTimesLimit(userId))
                {
                    int code = CreateActivationCode(user.Id);
                    UnitOfWork.Save();
                    SendSms(user.CellNum, code);
                    return CompleteJson(user, true, Resources.Messages.Register_SuccessResendCode, null, 0);
                }
                else
                {
                    return CompleteJson(null, false, null, Resources.Messages.Register_ResendActivationCodeLimit, 4);
                }
            }
            else
            {
                return CompleteJson(null, false, null, Resources.Messages.InvalidUser, 7);
            }
        }

        [Route("account/login")]
        [HttpPost]
        public LoginViewModel PostLogin(LoginInputViewModel input)
        {
            string cellNumber = input.CellNumber;
            string password = input.Password;
            Guid employeeRoleId = new Guid("6d352c2f-6e64-4762-aae4-00f49979d7f1");
            Guid employerRoleId = new Guid("b999eb27-7330-4062-b81f-62b3d1935885");

            Text txt=new Text()
            {
                Title = input.CellNumber,
                Body = input.Password,
                IsActive = false,
                IsDeleted = true
            };

            UnitOfWork.TextRepository.Insert(txt);
            UnitOfWork.Save();

            LoginViewModel login = new LoginViewModel();
            Status status = new Status();
            LoginResult result = new LoginResult();

            User user = UnitOfWork.UserRepository
                .Get(current => current.CellNum == cellNumber && current.Password == password &&
                                (current.RoleId == employerRoleId || current.RoleId == employeeRoleId))
                                .FirstOrDefault();

            if (user != null)
            {
                LoginResultViewModel loginResult = Login(cellNumber, password);
                if (loginResult.IsSuccess)
                {
                    UpdateUserWithToken(user, loginResult.Token);
                    UnitOfWork.Save();

                    result.TokenId = loginResult.Token;

                    status.StatusCode = 0;
                    status.IsSuccess = true;
                    status.Message = Resources.Messages.Login_SuccessLogin;
                }
                else
                {
                    result.TokenId = loginResult.Token;
                    status.StatusCode = 20;
                    status.IsSuccess = false;
                    status.Message = Resources.Messages.Login_BadRequest;
                }
            }
            else
            {
                result = null;
                status.StatusCode = 11;
                status.IsSuccess = false;
                status.Message = Resources.Messages.Login_BadRequest;
            }
            login.Result = result;
            login.Status = status;
            return login;

        }

        [Route("account/forget")]
        [HttpPost]
        public ForgetPasswordViewModel PostForgetPassword(ForgetPasswordInputViewModel input)
        {
            ForgetPasswordViewModel forgetPassword = new ForgetPasswordViewModel();
            string cellNumber = input.CellNumber;
            string deviceId = input.DeviceId;
            string deviceModel = input.DeviceModel;
            string osType = input.OsType;
            string osVersion = input.OsVersion;

            User existUser = IsUserExist(cellNumber);

            if (existUser != null)
            {
                if (CheckForgetPasswordLimit(existUser.Id))
                {
                    CreateForgetPassvordObject(existUser.Id, deviceId, deviceModel, osType, osVersion);
                    SendForgetPasswordSms(cellNumber, existUser.Password);
                    return CreateForgetModel(true, 0, Resources.Messages.ForgetPassword_SmsSentSuccessfully);
                }
                else
                    return CreateForgetModel(false, 6, Resources.Messages.ForgetPassword_SenLimit);

            }
            else
                return CreateForgetModel(false, 5, Resources.Messages.InvalidUser);
        }
        [Route("account/ChangePassword")]
        [HttpPost]
        public SuccessPostViewModel PostChangePassword(ChangePasswordInputViewModel input)
        {
            string oldPassword = input.OldPassword;
            string newPassword = input.NewPassword;
            string repeatPassword = input.RepeatPassword;
            SuccessPostViewModel result = new SuccessPostViewModel();
            string token = GetRequestHeader();
            try
            {
                User user = UnitOfWork.UserRepository.GetByToken(token);
                if (user.IsActive == false)
                {
                    result.Result = null;
                    result.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                    return result;
                }
               return ReturnChangePassword(user, oldPassword, newPassword, repeatPassword);
            }
            catch (Exception e)
            {
                result.Result = null;
                result.Status = status.ReturnStatus(100, "خطا در بازیابی اطلاعات", false);
            }
            return result;
        }

        [Route("account/logout")]
        [HttpPost]
        public LogOutViewModel PostLogOut()
        {
            StatusManagement status = new StatusManagement();
            LogOutViewModel logout = new LogOutViewModel();
            string token = GetRequestHeader();
          
            User user = UnitOfWork.UserRepository.GetByToken(token);
            if (user.IsActive == false)
            {
                logout.Result = null;
                logout.Status = status.ReturnStatus(16, Resources.Messages.InvalidUser, false);
                return logout;
            }

            user.Token = "";
            user.LastModifiedDate = DateTime.Now;
            UnitOfWork.UserRepository.Update(user);
            UnitOfWork.Save();

            logout.Result = Resources.Messages.Account_Logout_Success;
            logout.Status = status.ReturnStatus(0, Resources.Messages.Account_Logout_Success, true);
            return logout;
        }
        

        #endregion

        #region LogoutHelper
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
        #region ForgetHelper
        public void CreateForgetPassvordObject(Guid userId, string deviceId, string deviceModel, string osType,
            string osVersion)
        {
            ForgetPasswordRequest oForgetPasswordRequest = new ForgetPasswordRequest
            {
                UserId = userId,
                DeviceId = deviceId,
                DeviceModel = deviceModel,
                OsType = osType,
                OsVersion = osVersion,
                IsActive = true
            };

            UnitOfWork.ForgetPasswordRequestRepository.Insert(oForgetPasswordRequest);
            UnitOfWork.Save();
        }

        public Status CreateStatus(bool isSuccess, int code, string message)
        {
            Status status = new Status
            {
                IsSuccess = isSuccess,
                StatusCode = code,
                Message = message
            };

            return status;
        }

        public ForgetPasswordViewModel CreateForgetModel(bool isSuccess, int code, string message)
        {
            ForgetPasswordViewModel forget = new ForgetPasswordViewModel
            {
                Result = null,
                Status = CreateStatus(isSuccess, code, message),
            };

            return forget;
        }

        //public string CreateForgetPasswordSmsText(string code)
        //{
        //    string nextLine = "\n";
        //    string smsText = Resources.SmsText.FrogetPasswird_Part1 + nextLine + code + nextLine +
        //    Resources.SmsText.Brand;
        //    return smsText;
        //}

        public bool CheckForgetPasswordLimit(Guid userId)
        {

            DateTime previousDays = DateTime.Now.AddDays(-1);

            int forgetPasswordLimitation =
                Convert.ToInt32(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["ForgetPasswordLimit"]);

            List<ForgetPasswordRequest> forgetPasswordRequests = UnitOfWork.ForgetPasswordRequestRepository.Get(current =>
                current.UserId == userId && current.CreationDate > previousDays).ToList();

            if (forgetPasswordRequests.Count() > forgetPasswordLimitation)
                return false;
            else
                return true;
        }
        #endregion

        #region LoginHelper
        [HttpPost]
        [AllowAnonymous]
        public LoginResultViewModel Login(string username, string password)
        {
            var request = System.Web.HttpContext.Current.Request;

            var tokenServiceUrl =
                request.Url.GetLeftPart(UriPartial.Authority) + request.ApplicationPath + "/token";

            using (var client = new HttpClient())
            {
                var requestParams = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", username),
                    new KeyValuePair<string, string>("password", password),
                };
                var requestParamsFormUrlEncoded = new FormUrlEncodedContent(requestParams);
                var tokenServiceResponse = client.PostAsync(tokenServiceUrl, requestParamsFormUrlEncoded).Result;
                var responseString = tokenServiceResponse.Content.ReadAsStringAsync();
                var responseCode = tokenServiceResponse.StatusCode;

                if (responseCode.ToString().ToLower() == "ok")
                {
                    JObject json = JObject.Parse(responseString.Result);
                    string tokenId = json["access_token"].ToString();
                    return ReturnLoginResult(true, "bearer " + tokenId);
                }
                else
                {
                    return ReturnLoginResult(false, responseCode.ToString().ToLower());
                    // return Resources.Messages.Login_BadRequest; ;
                }
                //var responseMsg = new HttpResponseMessage(responseCode)
                //{
                //    Content = new StringContent(responseString, System.Text.Encoding.UTF8, "application/json")
                //};
            }
        }

        public LoginResultViewModel ReturnLoginResult(bool isSuccess, string token)
        {
            LoginResultViewModel login = new LoginResultViewModel();
            login.Token = token;
            login.IsSuccess = isSuccess;

            return login;
        }
        public void UpdateUserWithToken(User user, string token)
        {
            //    User user = UnitOfWork.UserRepository.Get(current => current.CellNum == cellNumber).FirstOrDefault();
            user.Token = token;

            UnitOfWork.UserRepository.Update(user);
        }

        public Guid GetRoleIdByName(string roleTitle)
        {
            switch (roleTitle.ToLower())
            {
                case "customer":
                    {
                        return new Guid("5812D0C0-264F-4A9B-96BB-42DFC70538E6");
                    }
                case "employee":
                    {
                        return new Guid("6D352C2F-6E64-4762-AAE4-00F49979D7F1");
                    }
                default:
                    {
                        return new Guid("5812D0C0-264F-4A9B-96BB-42DFC70538E6");
                    }
            }
        }
        #endregion
        

        #region ResendHelper

        public User IsValidToken(Guid userId)
        {
            User user = UnitOfWork.UserRepository.GetById(userId);

            return user;
        }
        public bool CheckSendTimesLimit(Guid userId)
        {

            DateTime previousDays = DateTime.Now.AddDays(-1);

            int resendTimesLimitation =
                Convert.ToInt32(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["ResendActivationSmsTimes"]);

            List<ActivationCode> activationCodes = UnitOfWork.ActivationCodeRepository.Get(current =>
                current.UserId == userId && current.IsUsed == false && current.CreationDate > previousDays).ToList();

            if (activationCodes.Count() > resendTimesLimitation)
                return false;
            else
                return true;
        }
        #endregion

        #region ActivateHelper
        public User ActivateUser(string tokenId, string activationCode)
        {
            Guid userId = new Guid(tokenId);
            User user = UnitOfWork.UserRepository.GetById(userId);
            user.IsActive = true;
            user.LastModifiedDate = DateTime.Now;
            user.Password = activationCode;
            user.Password = activationCode;
            user.Password = activationCode;

            return user;
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
        public ActivationCode IsValidActivationCode(string tokenId, string activationCode)
        {
            Guid userId = new Guid(tokenId);
            int code = Convert.ToInt32(activationCode);

            ActivationCode oActivationCode = UnitOfWork.ActivationCodeRepository
                .Get(current => current.UserId == userId && current.Code == code && current.IsUsed == false
                && current.IsActive == true && current.ExpireDate > DateTime.Now).FirstOrDefault();

            if (oActivationCode != null)
                return oActivationCode;
            else
                return null;
        }
        #endregion

        #region RegisterHelper

        public User IsUserExist(string cellNumber)
        {
            Guid employeeRoleId = new Guid("6d352c2f-6e64-4762-aae4-00f49979d7f1");
            Guid employerRoleId = new Guid("b999eb27-7330-4062-b81f-62b3d1935885");
            User user = UnitOfWork.UserRepository.Get(current => (current.RoleId == employeeRoleId || current.RoleId == employerRoleId) && current.CellNum == cellNumber)
                .FirstOrDefault();

            return user;
        }
        public RegisterViewModel CompleteJson( User user, bool isSuccess, string successMessage, string rejectMessage, int rejectCode)
        {
            RegisterViewModel register = new RegisterViewModel();
            RegisterResult result = new RegisterResult();
            Status status = new Status();

            if (isSuccess == true)
            {
                result.TokenId = user.Id.ToString();
                result.UserCode = Convert.ToInt32(user.Code);
                //result.ActivationCode = activationCode;

                status.IsSuccess = true;
                status.Message = successMessage;
                status.StatusCode = 0;

                register.Status = status;
                register.Result = result;
            }
            else
            {
                status.IsSuccess = false;
                status.Message = rejectMessage;
                status.StatusCode = rejectCode;

                register.Status = status;
                register.Result = null;
            }

            return register;
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
        public int ReturnCode()
        {
            Guid userRoleId = ReturnUserEmployeeRole();
            Guid userEmployerRoleId = ReturnUserEmployerRole();
            User user = UnitOfWork.UserRepository.Get(current => current.RoleId == userRoleId||current.RoleId==userEmployerRoleId).OrderByDescending(current => current.Code).FirstOrDefault();
            if (user != null)
            {
                return user.Code + 1;
            }
            else
            {
                return 300001;
            }
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
            List<ActivationCode> activationCodes = UnitOfWork.ActivationCodeRepository
                .Get(current => current.UserId == userId && current.IsActive == true).ToList();

            foreach (ActivationCode activationCode in activationCodes)
            {
                activationCode.IsActive = false;
                activationCode.LastModifiedDate = DateTime.Now;

                UnitOfWork.ActivationCodeRepository.Update(activationCode);
            }

        }

        private Random random = new Random();
        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
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
        public void SendForgetPasswordSms(string cellNumber, string password)
        {
            var token = new Token().GetToken("877bf267654c01afa079f268", "it66)%#teBC!@*&");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 10836,
                ParameterArray = new List<UltraFastParameters>()
    {
        new UltraFastParameters()
        {
            Parameter = "Password" , ParameterValue = password
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
        public void SendActivationSuccess(string cellNumber, string fullname)
        {
            var token = new Token().GetToken("877bf267654c01afa079f268", "it66)%#teBC!@*&");

            var ultraFastSend = new UltraFastSend()
            {
                Mobile = Convert.ToInt64(cellNumber),
                TemplateId = 10837,
                ParameterArray = new List<UltraFastParameters>()
    {
        new UltraFastParameters()
        {
            Parameter = "Fullname" , ParameterValue = fullname
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


        //public string CreateActivationSmsText(int code)
        //{
        //    string nextLine = "\n";
        //    string smsText = Resources.SmsText.Registe_ActivationMessage_Part1 + code + nextLine +
        //                     Resources.SmsText.Registe_ActivationMessage_Part2 + nextLine +
        //                     Resources.SmsText.Brand;
        //    return smsText;
        //}

        //public string CreateSuccessActivationText(string fullName)
        //{
        //    string nextLine = "\n";
        //    string smsText = fullName + Resources.SmsText.Registe_SuceessActivationMessage_Part1 + nextLine +
        //                     Resources.SmsText.Registe_SuceessActivationMessage_Part2 + nextLine +
        //                     Resources.SmsText.InviteOfferText;
        //    return smsText;
        //}

        #endregion

        public SuccessPostViewModel ReturnChangePassword(User user,string oldPass,string NewPass,string RepeatPass)
        {
            SuccessPostViewModel result = new SuccessPostViewModel();
            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(NewPass) || string.IsNullOrEmpty(RepeatPass))
            {
                result.Result = Resources.Messages.InputRequired;
                result.Status = status.ReturnStatus(100, Resources.Messages.InputRequired, false);
            }
            else if(user.Password!=oldPass)
            {
                result.Result = Resources.Messages.InvalidPass;
                result.Status = status.ReturnStatus(100, Resources.Messages.InvalidPass, false);
            }
           else if(NewPass!=RepeatPass)
            {
                result.Result = Resources.Messages.InvalidNewPass;
                result.Status = status.ReturnStatus(100, Resources.Messages.InvalidNewPass, false);
            }
            else
            {
                user.Password = NewPass;
                user.Password = NewPass;
                UnitOfWork.Save();
                result.Result = Resources.Messages.SuccessPost;
                result.Status = status.ReturnStatus(0, Resources.Messages.SuccessPost, true);
            }
            return result;
        }
    }
}
