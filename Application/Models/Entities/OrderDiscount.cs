using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class OrderDiscount:BaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid DiscountCodeId { get; set; }
        [Display(Name = "IsUse", ResourceType = typeof(Resources.Models.DiscountCode))]
        public bool IsUse { get; set; }
        [Display(Name = "UsingDate", ResourceType = typeof(Resources.Models.DiscountCode))]
        public DateTime? UsingDate { get; set; }
        public virtual Order Order { get; set; }
        public virtual DiscountCode DiscountCode { get; set; }
    }
}
