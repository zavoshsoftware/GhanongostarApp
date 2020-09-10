using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class VideoDetailViewModel : _BaseViewModel
    {
        public Product Product { get; set; }
        public List<HomeProducts> SideBarProducts { get; set; }
        public List<SidebarProductGroupViewModel> SideBarProductGroups { get; set; }
    }
}