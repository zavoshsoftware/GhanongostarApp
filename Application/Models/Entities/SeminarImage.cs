using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SeminarImage : BaseEntity
    {
        public string ImageAlt { get; set; }
        public Guid SeminarId { get; set; }
        public virtual Seminar Seminar { get; set; }
        public string ImageUrl { get; set; }
        
    }
}
