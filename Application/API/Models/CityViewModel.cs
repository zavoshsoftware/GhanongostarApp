using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class CityViewModel:BaseViewModel
    {
        public List<CityItem> Result { get; set; }
    }
    public class CityItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
    }
}