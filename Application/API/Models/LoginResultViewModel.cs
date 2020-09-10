using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class LoginResultViewModel
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
    }
}