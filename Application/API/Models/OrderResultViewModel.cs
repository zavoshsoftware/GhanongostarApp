using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class OrderResultViewModel:BaseViewModel
    {
        public OrderItemViewModel Result { get; set; }
    }

    public class OrderItemViewModel
    {
        public string PaymentLink { get; set; }
    }
}