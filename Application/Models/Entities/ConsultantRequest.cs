﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ConsultantRequestForm : BaseEntity
    {
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "شرکت")]
        public string Company { get; set; }
        [Display(Name = "نوع فعالیت")]
        public string ActionType { get; set; }
        [Display(Name = "تعداد پرسنل")]
        public string EmployeeQuantity { get; set; }
        [Display(Name = "تلفن تماس")]
        public string ContactNumber { get; set; }
        [Display(Name = "پیغام")]
        public string Message { get; set; }
    }
}
