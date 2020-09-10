using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class OrderDetail : BaseEntity
    {
        [Display(Name = "OrderFile", ResourceType = typeof(Resources.Models.Order))]
        public string OrderFile { get; set; }

        [Display(Name = "Fullname", ResourceType = typeof(Resources.Models.Order))]
        public string Fullname { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(Resources.Models.Order))]
        public int Quantity { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        public decimal Amount { get; set; }

        [Display(Name = "RawAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal RawAmount { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        internal class configuration : EntityTypeConfiguration<OrderDetail>
        {
            public configuration()
            {
                HasRequired(p => p.Order).WithMany(t => t.OrderDetails).HasForeignKey(p => p.OrderId);
                HasRequired(p => p.Product).WithMany(t => t.OrderDetails).HasForeignKey(p => p.ProductId);
            }
        }


        [NotMapped]
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        public string AmountStr
        {
            get { return Amount.ToString("N0"); }
        }

    }
}
