using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SiteBlogImage : BaseEntity
    {
        public string ImageAlt { get; set; }
        public Guid SiteBlogId { get; set; }
        public virtual SiteBlog SiteBlog { get; set; }
        public string ImageUrl { get; set; }
        internal class configuration : EntityTypeConfiguration<SiteBlogImage>
        {
            public configuration()
            {
                HasRequired(p => p.SiteBlog).WithMany(t => t.SiteBlogImages).HasForeignKey(p => p.SiteBlogId);
            }
        }
    }
}
