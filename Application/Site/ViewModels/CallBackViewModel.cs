using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class CallBackViewModel  :_BaseViewModel
    {
        public bool IsSuccess { get; set; }
        public string RefrenceId { get; set; }
        public string OrderCode { get; set; }
        public string DownloadLink { get; set; }
        public string ProductType { get; set; }
    }
}