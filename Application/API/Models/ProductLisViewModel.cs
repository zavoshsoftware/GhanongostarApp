using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProductLisViewModel:BaseViewModel
    {
        public List<ProductListItemViewModel> Result { get; set; }
    }

    public class ProductListItemViewModel
    {
        public string Code { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string IsFree { get; set; }
        public string Amount { get; set; }
        public string Point { get; set; }
        public bool IsOwner { get; set; }
        public bool Ispreviously { get; set; }
        public string TypeName { get; set; }
    }
    
}