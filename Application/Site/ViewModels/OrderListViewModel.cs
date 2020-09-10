using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderListViewModel:_BaseViewModel
    {
        public List<Order> Orders { get; set; }
        public User User { get; set; }
    }
}