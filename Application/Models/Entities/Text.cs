using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
   public class Text:BaseEntity
    {
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Text))]
        public string Title { get; set; }
        [Display(Name = "Body", ResourceType = typeof(Resources.Models.Text))]
        [AllowHtml]
        public string Body { get; set; }
    }
}
