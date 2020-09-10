using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resources;

namespace Models
{
    public class Page : BaseEntity
    {
        public Page()
        {
            PageCounts = new List<PageCount>();
        }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(100, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Title { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        [StringLength(100, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string Name { get; set; }

        public virtual ICollection<PageCount> PageCounts { get; set; }
    }
}
