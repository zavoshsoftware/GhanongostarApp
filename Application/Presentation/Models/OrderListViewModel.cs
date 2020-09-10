using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class OrderListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "نام مشتری")]
        public string FullName { get; set; }

        [Display(Name = "نوع محصول")]
        public string OrderTypeTitle { get; set; }

        [Display(Name = "کد سفارش")]
        public int Code { get; set; }

        [Display(Name = "مبلغ سفارش")]
        public string Amount { get; set; }

        [Display(Name = "مبلغ پرداختی")]
        public string TotalAmount { get; set; }

        [Display(Name = "پرداخت شده؟")]
        public bool IsPaid { get; set; }

        [Display(Name = "تاریخ پرداخت")]
        public DateTime? PaymentDate { get; set; }

        [Display(Name = "کد رهگیری")]
        public string ResCode { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreationDate { get; set; }

        [Display(Name = "محصول")]
        public string ProductTitle { get; set; }
    }
}