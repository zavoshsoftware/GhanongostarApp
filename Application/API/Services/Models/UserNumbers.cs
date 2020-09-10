using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Services.Models
{
    public class UserNumbers
    {
        public MyBase MyBase { get; set; }
        public List<Datum> Data { get; set; }
    }
    public class MyBase
    {
        public string Value { get; set; }
        public int RetStatus { get; set; }
        public string StrRetStatus { get; set; }
    }

    public class Datum
    {
        public string Number { get; set; }
    }

 
}