using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class DiscountViewModel:BaseViewModel
    {
        public DiscountResult Result { get; set; }
    }
    public class DiscountResult
    {
        public Guid DiscountId { get; set; }
        public string DiscountAmount { get; set; }
    }
}