using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
    public class EmpClubQuestion : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        [Display(Name = "موضوع سوال")]
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "متن سوال")]
        public string Question { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "پاسخ")]
        public string Response { get; set; }
        [Display(Name = "زمان پاسخ")]
        public DateTime ResponseDate { get; set; }
        [Display(Name = "پاسخ صوتی")]
        public string VoiceResponse { get; set; }
    }
}
