using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Input
{
    public class CalculateInputViewModel
    {
        public string Sallary { get; set; }
        public string StartMonth { get; set; }
        public string StartYear { get; set; }
        public string FinishMonth { get; set; }
        public string FinishYear { get; set; }
    }
}