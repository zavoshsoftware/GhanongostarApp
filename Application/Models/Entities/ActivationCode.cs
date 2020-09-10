using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ActivationCode : BaseEntity
    {
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.ActivationCode))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }
        [Display(Name = "ExpireDate", ResourceType = typeof(Resources.Models.ActivationCode))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public DateTime ExpireDate { get; set; }
        [Display(Name = "IsUsed", ResourceType = typeof(Resources.Models.ActivationCode))]
        public bool IsUsed { get; set; }
        [Display(Name = "UsingDate", ResourceType = typeof(Resources.Models.ActivationCode))]
        public DateTime? UsingDate { get; set; }

        [Display(Name = "DeviceId", ResourceType = typeof(Resources.Models.ActivationCode))]
        [StringLength(200, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string DeviceId { get; set; }
        [Display(Name = "DeviceModel", ResourceType = typeof(Resources.Models.ActivationCode))]
        [StringLength(50, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string DeviceModel { get; set; }
        [Display(Name = "DeviceType", ResourceType = typeof(Resources.Models.ActivationCode))]
        [StringLength(50, ErrorMessage = "طول {0} نباید بیشتر از {1} باشد")]
        public string OsType { get; set; }
        [Display(Name = "OsVersion", ResourceType = typeof(Resources.Models.ActivationCode))]
        public string OsVersion { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }


        internal class Configuration : EntityTypeConfiguration<ActivationCode>
        {
            public Configuration()
            {
                HasRequired(p => p.User)
                    .WithMany(j => j.ActivationCodes)
                    .HasForeignKey(p => p.UserId);
            }
        }
    }
}
