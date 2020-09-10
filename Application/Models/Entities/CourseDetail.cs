using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CourseDetail:BaseEntity
    {
        [Display(Name = "ProductId", ResourceType = typeof(Resources.Models.CourseDetail))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid ProductId { get; set; }

        [Display(Name = "SessionNumber", ResourceType = typeof(Resources.Models.CourseDetail))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int SessionNumber { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.CourseDetail))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }

        [Display(Name = "Summery", ResourceType = typeof(Resources.Models.CourseDetail))]
        [DataType(DataType.MultilineText)]
        public string Summery { get; set; }

        [Display(Name = "VideoUrl", ResourceType = typeof(Resources.Models.CourseDetail))]
        public string VideoUrl { get; set; }

        [Display(Name = "Body", ResourceType = typeof(Resources.Models.CourseDetail))]
        public string Body { get; set; }

        [Display(Name = "ThumbnailImageUrl", ResourceType = typeof(Resources.Models.CourseDetail))]
        public string ThumbnailImageUrl { get; set; }

        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<CourseDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.CourseDetails).HasForeignKey(p => p.ProductId);
            }
        }
    }
}
