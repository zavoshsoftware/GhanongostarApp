using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductType:BaseEntity
    {
        public ProductType()
        {
            Products = new List<Product>();
            Orders = new List<Order>();

        }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductType))]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Models.ProductType))]
        [StringLength(250, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
