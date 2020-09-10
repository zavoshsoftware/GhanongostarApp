using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class LoginViewModel:BaseViewModel
    {
        public LoginResult Result { get; set; }
    }

    public class LoginResult
    {
        public string TokenId { get; set; }

    }





}