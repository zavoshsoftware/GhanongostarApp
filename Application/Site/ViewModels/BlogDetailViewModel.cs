using System.Collections.Generic;
using Models;

namespace ViewModels
{
    public class BlogDetailViewModel : _BaseViewModel
    {
        public SiteBlog SiteBlog { get; set; }
        public List<SiteBlog> SideBarBlogs { get; set; }
        public List<SiteBlogCategory> SideBarSiteBlogCategories { get; set; }
        public List<HomeProducts> SideBarProducts { get; set; }
        public List<SiteBlogImage> SiteBlogImages { get; set; }

    }
}