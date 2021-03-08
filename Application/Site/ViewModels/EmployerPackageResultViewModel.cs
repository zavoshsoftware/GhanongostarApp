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
        public  EmpClubVideos  Videos { get; set; }
      
        public List<EmpClubProduct> Instructions { get; set; }
        public List<EmpClubQuestion> Questions { get; set; }
    }

    public class EmpClubVideos
    {
        public List<EmpClubProduct> GhanonKarVideos { get; set; }
        public List<EmpClubProduct> TaminVideos { get; set; }
        public List<EmpClubProduct> VatVideos { get; set; }
    }
}