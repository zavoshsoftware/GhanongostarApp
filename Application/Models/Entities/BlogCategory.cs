using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BlogCategory : BaseEntity
    {
        public BlogCategory()
        {
            Blogs = new List<Blog>();
        }
        public string Title { get; set; }
        public int Order { get; set; }

        public virtual ICollection<Blog> Blogs { get; set; }
    }
}
