using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Viewmodels
{
    public class ForgetPasswordInputViewModel
    {
        public string CellNumber { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string OsType { get; set; }
        public string OsVersion { get; set; }
    }
}