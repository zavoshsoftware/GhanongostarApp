using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace API.Models
{
    public class SupportRequestViewModel:BaseViewModel
    {
        public SupportItems Result { get; set; }
    }
    public class SupportItems
    {
        public List<KeyValueViewModel> KeyValueList { get; set; }
        public bool IsCustomer { get; set; }
        public List<SupportType> SupportRequestTypeList { get; set; }
    }
    public class SupportType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        
    }
}