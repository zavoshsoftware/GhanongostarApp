using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SiteBlogCategory : BaseEntity
    {
        public SiteBlogCategory()
        {
            SiteBlogs = new List<SiteBlog>();
        }
        public string Title { get; set; }
        public int Order { get; set; }
        public string UrlParam { get; set; }

        public virtual ICollection<SiteBlog> SiteBlogs { get; set; }
    }
}
