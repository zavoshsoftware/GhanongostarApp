using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductListViewModel : _BaseViewModel
    {
        public List<Product> Products { get; set; }
        public List<SidebarProductGroupViewModel> SideBarProductGroups { get; set; }
        public List<HomeProducts> SideBarProducts { get; set; }

    }
}