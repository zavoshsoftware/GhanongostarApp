using System;
using System.Data.Entity.ModelConfiguration;
using System.Web.Mvc;

namespace Models
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public int Order { get; set; }

        [AllowHtml]
        public string Body { get; set; }
        public string ImageUrl { get; set; }
        public Guid BlogCategoryId { get; set; }
        public virtual BlogCategory BlogCategory { get; set; }

        internal class configuration : EntityTypeConfiguration<Blog>
        {
            public configuration()
            {
                HasRequired(p => p.BlogCategory).WithMany(t => t.Blogs).HasForeignKey(p => p.BlogCategoryId);
            }
        }
    }
}
