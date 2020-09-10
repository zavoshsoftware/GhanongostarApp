using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProductUserListViewModel:BaseViewModel
    {
        public List<ProductUserListItemViewModel> Result { get; set; }
    }

    public class ProductUserListItemViewModel
    {
        public string Code { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string IsFree { get; set; }
        public string Amount { get; set; }
        public string Point { get; set; }
        public bool Ispreviously { get; set; }
        public string ProductTypeTitle { get; set; }
        public string ProductTypeName { get; set; }
    }
}