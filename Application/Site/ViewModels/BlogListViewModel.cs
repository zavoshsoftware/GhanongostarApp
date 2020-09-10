using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BlogListViewModel : _BaseViewModel
    {
        public List<SiteBlog> SiteBlogs { get; set; }
        public List<ProductGroup> SideBarProductGroups { get; set; }
        public SiteBlogCategory SiteBlogCategory { get; set; }
    }
}