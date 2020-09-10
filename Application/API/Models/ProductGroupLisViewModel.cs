using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProductGroupLisViewModel : BaseViewModel
    {
        public List<ProductGroupListItemViewModel> Result { get; set; }
    }

    public class ProductGroupListItemViewModel
    {
        public string Code { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
    }
}