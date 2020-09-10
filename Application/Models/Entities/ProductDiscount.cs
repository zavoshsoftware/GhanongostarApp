using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductDiscount:BaseEntity
    {
        public Guid DiscountCodeId { get; set; }
        public Guid ProductId { get; set; }
        public DiscountCode DiscountCode { get; set; }
        public Product Product { get; set; }


        internal class configuration : EntityTypeConfiguration<ProductDiscount>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.ProductDiscounts).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.DiscountCode).WithMany(t => t.ProductDiscounts).HasForeignKey(p => p.DiscountCodeId);
            }
        }
    }
}
