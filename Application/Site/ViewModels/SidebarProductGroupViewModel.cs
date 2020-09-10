using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class SidebarProductGroupViewModel
    {
        public ProductGroup ProductGroup { get; set; }
        public int ProductCount { get; set; }
    }
}