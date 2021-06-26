using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class SeminarListViewModel : _BaseViewModel
    {
        public List<Seminar> Seminars { get; set; }
        public List<SidebarProductGroupViewModel> SideBarProductGroups { get; set; }
        public List<HomeProducts> SideBarProducts { get; set; }

    }
}