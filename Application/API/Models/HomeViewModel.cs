using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class HomeViewModel:BaseViewModel
    {
        public HomeItems Result { get; set; }
    }

    public class HomeItems
    {
        public List<string> Images { get; set; }
        public string MessageCount { get; set; }
    }
}