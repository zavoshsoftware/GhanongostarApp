using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class SeminarTeacher : BaseEntity
    {
        public string Title { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        public Guid SeminarId { get; set; }
        public virtual Seminar Seminar { get; set; }
    }
}
