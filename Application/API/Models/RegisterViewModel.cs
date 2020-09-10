using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class RegisterViewModel:BaseViewModel
    {
        public RegisterResult Result { get; set; }
    }

    public class RegisterResult
    {
        public int UserCode { get; set; }
        public string  TokenId { get; set; }
        //public string ActivationCode { get; set; }
    }

  

}