using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class BlogListViewModel : BaseViewModel
    {
        public List<BlogItemViewModel> Result { get; set; }
    }

    public class BlogItemViewModel
    {
        public string Title { get; set; }
        public string BlogCategoryTitle { get; set; }
        public string ImageUrl { get; set; }
        public string WebViewUrl { get; set; }
        public string image { get; set; }
    }
}