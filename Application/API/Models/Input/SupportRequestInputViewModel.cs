using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Input
{
    public class SupportRequestInputViewModel
    {
        public Guid TypeId { get; set; }
        public string Body { get; set; }
    }
}