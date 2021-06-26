using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class   Seminar:BaseEntity
    {
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Display(Name = "Body", ResourceType = typeof(Resources.Models.Product))]
        [AllowHtml]
        public string Body { get; set; }

        [Display(Name = "تاریخ برگزاری")]
        public string ExecuteDate { get; set; }

        [Display(Name = "همایش آینده؟")]
        public bool IsNew { get; set; }

        [Display(Name = "مکان برگزاری")]
        public string Place { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        public virtual ICollection<SeminarTeacher> SeminarTeachers { get; set; }
        public virtual ICollection<SeminarImage> SeminarImages { get; set; }




        //[Display(Name = "تاریخ برگزاری")]
        //[NotMapped]
        //public string ExecuteDateStr {
        //    get
        //    {
        //        //  return "hi";
        //        System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
        //        string year = pc.GetYear(ExecuteDate).ToString().PadLeft(4, '0');
        //        string month = pc.GetMonth(ExecuteDate).ToString().PadLeft(2, '0');
        //        string day = pc.GetDayOfMonth(ExecuteDate).ToString().PadLeft(2, '0');
        //        return String.Format("{0}/{1}/{2}", year, month, day)  ;
        //    }
        //}

    }
}
