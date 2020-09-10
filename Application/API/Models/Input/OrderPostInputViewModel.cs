using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.Input
{
    public class OrderPostInputViewModel
    {
        public string ProductCode { get; set; }
        public List<string> FullName { get; set; }
        public string Email { get; set; }
        public Guid CityId { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public Guid? DiscountCodeId { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}