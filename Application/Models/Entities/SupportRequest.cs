using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models
{
  public  class SupportRequest:BaseEntity
    {
        [Display(Name = "TypeId", ResourceType = typeof(Resources.Models.SupportRequest))]
        public Guid SupportRequestTypeId { get; set; }
         public virtual SupportRequestType Type { get; set; }

        [Display(Name = "Body", ResourceType = typeof(Resources.Models.SupportRequest))]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Models.SupportRequest))]
        public int Status { get; set; }

        [Display(Name = "Response", ResourceType = typeof(Resources.Models.SupportRequest))]
        [DataType(DataType.MultilineText)]
        public string Response { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.SupportRequest))]
        public int Code { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        internal class configuration : EntityTypeConfiguration<SupportRequest>
        {
            public configuration()
            {
                HasRequired(p => p.Type).WithMany(t => t.SupportRequests).HasForeignKey(p => p.SupportRequestTypeId);
                HasRequired(p => p.User).WithMany(t => t.SupportRequests).HasForeignKey(p => p.UserId);
            }
        }
    }
}
