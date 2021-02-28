using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class EmployerPackageResultViewModel : _BaseViewModel
    {

        public List<HomeProducts> SideBarProducts { get; set; }
        public List<SidebarProductGroupViewModel> SideBarProductGroups { get; set; }
        public List<EmpClubProduct> Forms { get; set; }
        public List<EmpClubProduct> Videos { get; set; }
        public List<EmpClubProduct> Instructions { get; set; }
        public List<EmpClubQuestion> Questions { get; set; }
    }
}