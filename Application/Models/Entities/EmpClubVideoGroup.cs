using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmpClubVideoGroup : BaseEntity
    {
        [Display(Name="گروه ویدیو")]
        public string Title { get; set; }
        public string Name { get; set; }

        public virtual ICollection<EmpClubProduct> EmpClubProducts { get; set; }
    }
}
