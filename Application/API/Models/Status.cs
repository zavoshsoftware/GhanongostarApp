using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Status
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
    }
}