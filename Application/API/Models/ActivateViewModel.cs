using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ActivateViewModel : BaseViewModel
    {
       public ActivateResult Result { get; set; }
    }

    public class ActivateResult
    {
        public string TokenId { get; set; }
    }
}