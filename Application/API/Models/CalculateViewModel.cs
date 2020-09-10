using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class CalculateViewModel:BaseViewModel
    {
        public CalculateItemsViewModel Result { get; set; }
    }

    public class CalculateItemsViewModel
    {
        public string Sanavat { get; set; }
        public string Morakhasi { get; set; }
        public List<KeyValueViewModel> Eydi { get; set; }
        public List<KeyValueViewModel> PayeSanavat { get; set; }
        public List<KeyValueViewModel> Olad { get; set; }
        public List<KeyValueViewModel> Bon { get; set; }
        public List<KeyValueViewModel> Maskan { get; set; }
        public string SanavatDescription { get; set; }
        public string MorkhasiDescription { get; set; }
    }

    public class KeyValueViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}