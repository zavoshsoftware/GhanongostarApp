using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class SupportRequestType:BaseEntity
    {
        public SupportRequestType()
        {
            SupportRequests = new List<SupportRequest>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.SupportRequestType))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public string Title { get; set; }
        [Display(Name = "Order", ResourceType = typeof(Resources.Models.SupportRequestType))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Order { get; set; }

        public virtual ICollection<SupportRequest> SupportRequests { get; set; }
    }
}
