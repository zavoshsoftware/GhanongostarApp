using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ProvinceViewModel:BaseViewModel
    {
         public List<ProvinceItem> Result { get; set; }
    }
    public class ProvinceItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}