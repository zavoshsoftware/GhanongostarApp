using System.ComponentModel.DataAnnotations;

namespace Models
{
   public class VersionHistory:BaseEntity
    {
        [Display(Name = "VersionNumber", ResourceType =typeof(Resources.Models.VersionHistory))]
        public string VersionNumber { get; set; }

        [Display(Name = "سیستم عامل")]
        public string Os { get; set; }

        [Display(Name = "IsNeccessary", ResourceType = typeof(Resources.Models.VersionHistory))]
        public bool IsNeccessary { get; set; }

        public string LatestStableVersion { get; set; }
        public bool IsBeta { get; set; }
    }
}
