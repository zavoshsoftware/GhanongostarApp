using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class VersionHistoryViewModel:BaseViewModel
    {
        public VersionItems Result { get; set; }
    }
    public class VersionItems
    {
        public string VersionNumber { get; set; }
        public string Link { get; set; }
        public bool IsNeccessary { get; set; }
        public string LatestStableVersion { get; set; }
        public bool IsBeta { get; set; }
    }
}