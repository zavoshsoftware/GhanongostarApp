using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmpClubProductGroup : BaseEntity
    {
        [Display(Name="گروه محتوا")]
        public string Title { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmpClubProduct> EmpClubProducts { get; set; }
    }
}
