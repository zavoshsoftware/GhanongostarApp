using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class FormInstagramLive:BaseEntity
    {
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
       
        [Display(Name = "ای دی اینستاگرام")]
        public string InstagramId { get; set; }
        [Display(Name = "تلفن تماس")]
        public string ContactNumber { get; set; }

        [Display(Name = "پرداخت شده؟")]
        public bool IsPaid { get; set; }

        [Display(Name = "کد سفارش")]
        public string OrderCode { get; set; }

        [Display(Name = "شماره پیگیری پرداخت")]
        public string SaleRefrenceId { get; set; }


    }
}
