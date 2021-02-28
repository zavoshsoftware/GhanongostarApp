using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProfileGetViewModel:BaseViewModel
    {
        public profileGetItemViewModel Result { get; set; }
    }

    public class profileGetItemViewModel
    {
        public string CellNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Rate { get; set; }
        public string InviteText { get; set; }
    }

  

}