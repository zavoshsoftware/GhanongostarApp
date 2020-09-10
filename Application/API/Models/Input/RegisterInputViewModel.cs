using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Input
{
    public class RegisterInputViewModel
    {
        public string FullName { get; set; }
        public string CellNumber { get; set; }
        public string IsEmployee { get; set; }
    }
}