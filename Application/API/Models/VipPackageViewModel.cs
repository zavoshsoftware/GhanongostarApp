using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
namespace API.Models
{
    public class VipPackageViewModel:BaseViewModel
    {
        public VipPackageItem Result { get; set; }
    }
    public class VipPackageItem
    {
        public bool HasVipPackage { get; set; }
        public List<VipPackageListItem> VipPackageList { get; set; }
        public List<ProductListItemViewModel> Products { get; set; }
    }
    public class VipPackageListItem
    {
        public string Title { get; set; }
        public int Month { get; set; }
        public decimal Price { get; set; }
        public int ProductCode { get; set; }
        public List<string> Features { get; set; }
        
    }
  
}