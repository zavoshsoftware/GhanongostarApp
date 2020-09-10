﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class CourseDetailListViewModel:BaseViewModel
    {
        public List<CourseDetailItemViewModel> Result { get; set; }
    }

    public class CourseDetailItemViewModel
    {
        public string SessionNumber { get; set; }
        public string Title { get; set; }
        public string Summery { get; set; }
        public string VideoUrl { get; set; }
        public string ThumbnailImageUrl { get; set; }
    }
}