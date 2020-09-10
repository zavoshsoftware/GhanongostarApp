using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class SiteBlog : BaseEntity
    {
        public SiteBlog()
        {
            SiteBlogImages=new List<SiteBlogImage>();
        }

        [Display(Name="عنوان مقاله")]
        public string Title { get; set; }
        [Display(Name="اولویت نمایش")]
        public int Order { get; set; }

        [AllowHtml]
        [Display(Name="متن")]
        public string Body { get; set; }
        [Display(Name="تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name="خلاصه")]
        public string Summery { get; set; }
        [Display(Name="پارامتر url")]
        public string UrlParam { get; set; }

        public virtual ICollection<SiteBlogImage> SiteBlogImages { get; set; }


        [Display(Name="گروه مقاله")]
        public Guid SiteBlogCategoryId { get; set; }
        public virtual SiteBlogCategory SiteBlogCategory { get; set; }

        public string OldUrlParam { get; set; }

    }
}
