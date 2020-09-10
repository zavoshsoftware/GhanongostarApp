using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Input
{
    public class ActivateInputViewModel
    {
        public string TokenId { get; set; }
        public string ActivationCode { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string OsType { get; set; }
        public string OsVersion { get; set; }
    }
}