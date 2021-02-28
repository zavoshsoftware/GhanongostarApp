using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class EmpClubProduct : BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "فایل")]
        public string FileUrl { get; set; }

        [Display(Name = "متن")]
        [AllowHtml]
        public string Body { get; set; }


        public Guid EmpClubProductGroupId { get; set; }
        public virtual EmpClubProductGroup EmpClubProductGroup { get; set; }
    }
}
