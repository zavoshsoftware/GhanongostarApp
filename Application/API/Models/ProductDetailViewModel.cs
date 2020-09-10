using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProductDetailViewModel : BaseViewModel
    {
        public ProductDetailItemViewModel Result { get; set; }
    }

    public class ProductDetailItemViewModel
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string IsFree { get; set; }
        public string Amount { get; set; }
        public string Point { get; set; }
        public string webViewUrl { get; set; }
        public string ProductFileUrl { get; set; }
        public string VideoThumb { get; set; }
    }
}