using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class HomeViewModel:_BaseViewModel
    {
        public List<SiteBlog> SiteBlogs { get; set; }

        public List<HomeProducts> HomeProducts { get; set; }

        public List<Product> LatestVideos { get; set; }
    }

    public class HomeProducts
    {
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Amount { get; set; }
        public bool IsInPromotion { get; set; }
        public string DiscountAmount { get; set; }
    }
}