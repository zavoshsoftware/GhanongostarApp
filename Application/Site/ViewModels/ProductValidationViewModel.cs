using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductValidationViewModel : _BaseViewModel
    {
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public string OrderDate { get; set; }
        public string UserFullName { get; set; }
        public Guid OrderId { get; set; }
    }
}