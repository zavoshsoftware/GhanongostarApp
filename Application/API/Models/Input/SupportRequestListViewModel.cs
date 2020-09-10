using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class SupportRequestListViewModel:BaseViewModel
    {
        public List<SupportListItem> Result { get; set; }
    }
    public class SupportListItem
    {
        public int Status { get; set; }
        public int Code { get; set; }
        public string SubmitDate { get; set; }
        public string Title { get; set; }
        public string Response { get; set; }
        public string Body { get; set; }
    }
}