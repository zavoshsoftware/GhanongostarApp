using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PageCount:BaseEntity
    {
        public Guid PageId { get; set; }
        public int Count { get; set; }
        public DateTime VisitDate { get; set; }
        public Guid? EntityId { get; set; }

        public virtual Page Page { get; set; }


        internal class configuration : EntityTypeConfiguration<PageCount>
        {
            public configuration()
            {
                HasOptional(p => p.Page).WithMany(t => t.PageCounts).HasForeignKey(p => p.PageId);
            }
        }
    }
}
