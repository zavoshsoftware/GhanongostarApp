using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Redirect:BaseEntity
    {
        public string OldUrl { get; set; }
        public string NewUrl { get; set; }
    }
}
