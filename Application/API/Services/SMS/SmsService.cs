using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using RestSharp;
using Services.Models;

namespace Services
{
    public class SmsService : ISmsService
    {
        private readonly string _username;
        private readonly string _password;
        public SmsService()
        {
            _username = WebConfigurationManager.AppSettings["SmsUserName"];
            _password = WebConfigurationManager.AppSettings["SmsPassword"];
            InitRestRequest();
        }

        public RestRequest RestRequest { get; set; }

        private void InitRestRequest()
        {
            this.RestRequest = new RestRequest(Method.POST);
            //   this.RestRequest.AddHeader("postman-token", "fcddb5f4-dc58-c7d5-4bf9-9748710f8789");
            this.RestRequest.AddHeader("cache-control", "no-cache");
            this.RestRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
        }
        public int DeliveryStatus(long recId)
        {
            string parameter = "username=" + _username + "&password=" + _password + "recID=" + recId;
            IRestResponse response = ExecuteRequest("GetDeliveries", parameter);
            SmsCredit content = new JavaScriptSerializer().Deserialize<SmsCredit>(response.Content);
            return Convert.ToInt32(content.Value);
        }
        public bool HasCredit(int smsQuantity)
        {
            string parameter = "username=" + _username + "&password=" + _password;
            IRestResponse response = ExecuteRequest("SmsGetCredit", parameter);
            SmsCredit content = new JavaScriptSerializer().Deserialize<SmsCredit>(response.Content);

            string val = content.Value.Split('.')[0];

            decimal credit = Convert.ToDecimal(content.Value.Split('.')[0]);

            if (smsQuantity <= credit)
                return true;
            else
                return false;
        }

        public decimal GetBaseAmount()
        {
            string parameter = "username=" + _username + "&password=" + _password;
            IRestResponse response = ExecuteRequest("SmsGetBasePrice", parameter);
            SmsCredit content = new JavaScriptSerializer().Deserialize<SmsCredit>(response.Content);

            return Convert.ToDecimal(content.Value) / 10;
        }
        public bool IsActiveNumber()
        {
            string number = WebConfigurationManager.AppSettings["SmsActiveNumber"];
            string parameter = "username=" + _username + "&password=" + _password;
            IRestResponse response = ExecuteRequest("SmsGetUserNumbers", parameter);
            UserNumbers content = new JavaScriptSerializer().Deserialize<UserNumbers>(response.Content);

            foreach (var datum in content.Data)
            {
                if (datum.Number == number)
                    return true;
            }
            return false;
        }
        public long SendSms(List<string> reciever, string message)
        {
            string number = WebConfigurationManager.AppSettings["SmsActiveNumber"];
            // string parameter = "username=" + _username + "&password=" + _password + "&to=" + ReturnStringReciever(reciever) + "&from=" + number + "&text=" + message + "&isflash=false";
            // IRestResponse response = ExecuteRequest("SmsSendSMS", parameter);

            var client = new RestClient(WebConfigurationManager.AppSettings["SmsSendSMS"]);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            // request.AddHeader("postman-token", "fcddb5f4-dc58-c7d5-4bf9-9748710f8789");
            request.AddHeader("cache-control", "no-cache");
            request.AddParameter("application/x-www-form-urlencoded", "username=" + _username + "&password=" + _password + "&to=" + ReturnStringReciever(reciever) + "&from=" + number + "&text=" + message + "&isflash=false", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);


            SmsCredit content = new JavaScriptSerializer().Deserialize<SmsCredit>(response.Content);
            return Convert.ToInt64(content.Value);
        }
        public IRestResponse ExecuteRequest(string clientApi, string parameter)
        {
            //this.RestRequest.AddHeader("postman-token", token);
            var client = new RestClient(WebConfigurationManager.AppSettings[clientApi]);
            this.RestRequest.AddParameter("application/x-www-form-urlencoded", parameter, ParameterType.RequestBody);
            IRestResponse response = client.Execute(this.RestRequest);

            return response;
        }
        public string ReturnStringReciever(List<string> recievers)
        {
            string reciever = null;
            foreach (var item in recievers)
            {
                if (reciever != null)
                    reciever = reciever + "," + item;
                else
                    reciever = item;
            }
            return reciever;
        }



        public int SendToList(List<string> reciever, string messageBody)
        {
            if (IsActiveNumber())
            {
                if (HasCredit(reciever.Count()))
                {
                    long recId = SendSms(reciever, messageBody);
                    return DeliveryStatus(recId);
                }
                else
                    return 600;
            }
            else
                return 700;


            // Log Sms Status and Sms 
        }



        public int SendToOne(string reciever, string messageBody)
        {
           
            if (IsActiveNumber())
            {
                List<string> recievers = new List<string>();
                recievers.Add(reciever);

                if (HasCredit(1))
                {
                    long recId = SendSms(recievers, messageBody);
                    return DeliveryStatus(recId);
                }
                else
                    return 600;
            }
            else
                return 700;


            // Log Sms Status and Sms 
        }
        //public Task<int> SendToOne(string reciever, string messageBody)
        //{
        //    //List<string> recievers = new List<string>();
        //    //recievers.Add(reciever);
        //    //long recId = SendSms(recievers, messageBody);
        //    //return DeliveryStatus(recId);
        //    if (IsActiveNumber())
        //    {
        //        List<string> recievers = new List<string>();
        //        recievers.Add(reciever);

        //        if (HasCredit(1))
        //        {
        //            long recId = SendSms(recievers, messageBody);
        //            return Task.FromResult(DeliveryStatus(recId));
        //        }
        //        else
        //            return Task.FromResult(600);
        //    }
        //    else
        //        return Task.FromResult(700);


        //    // Log Sms Status and Sms 
        //}
    }
}