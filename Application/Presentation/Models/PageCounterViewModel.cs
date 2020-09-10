using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class PageCounterViewModel
    {
        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }

        public List<PageCountItem> PageCounts { get; set; }
    }

    public class PageCountItem
    {
        [Display(Name = "عنوان صفحه")]
        public string Title { get; set; }

        [Display(Name = "زیر عنوان ")]
        public string SubTitle { get; set; }

        [Display(Name = "تعداد بازدید")]
        public int Count { get; set; }
    }
}