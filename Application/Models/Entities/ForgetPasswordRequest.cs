using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace Models
{
    public class ForgetPasswordRequest : BaseEntity
    {
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

        internal class Configuration : EntityTypeConfiguration<ForgetPasswordRequest>
        {
            public Configuration()
            {
                HasRequired(p => p.User)
                    .WithMany(j => j.ForgetPasswordRequests)
                    .HasForeignKey(p => p.UserId);
            }
        }

    }
}
