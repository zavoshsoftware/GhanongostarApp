using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class VipPackageFeature:BaseEntity
    {
        public Guid VipPackageId { get; set; }
        public VipPackage VipPackage { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.VipPackageFeature))]
        public string Title { get; set; }

        internal class configuration : EntityTypeConfiguration<VipPackageFeature>
        {
            public configuration()
            {
                HasRequired(p => p.VipPackage).WithMany(t => t.VipPackageFeatures).HasForeignKey(p => p.VipPackageId);
            }
        }
    }
}
