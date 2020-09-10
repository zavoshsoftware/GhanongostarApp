using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class QuestionConversation:BaseEntity
    {
        [Display(Name = "Order", ResourceType = typeof(Resources.Models.QuestionConversation))]
        public int Order { get; set; }
        [Display(Name = "Subject", ResourceType = typeof(Resources.Models.QuestionConversation))]
        public string Subject { get; set; }
        [Display(Name = "Body", ResourceType = typeof(Resources.Models.QuestionConversation))]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
        [Display(Name = "StatusCode", ResourceType = typeof(Resources.Models.QuestionConversation))]
        public int StatusCode { get; set; }

        [Display(Name = "UserId", ResourceType = typeof(Resources.Models.QuestionConversation))]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<QuestionConversation> QuestionConversations { get; set; }
        public virtual QuestionConversation Parent { get; set; }

        //[Display(Name = "ParentId", ResourceType = typeof(Resources.Models.QuestionConversation))]
        public Guid? ParentId { get; set; }


        internal class configuration : EntityTypeConfiguration<QuestionConversation>
        {
            public configuration()
            {
                HasRequired(p => p.Parent).WithMany(t => t.QuestionConversations).HasForeignKey(p => p.ParentId);
            }
        }
    }
}
