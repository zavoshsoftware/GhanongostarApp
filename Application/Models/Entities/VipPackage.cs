using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class VipPackage:BaseEntity
    {
        public VipPackage()
        {
            UserVipPackages = new List<UserVipPackage>();
            VipPackageFeatures = new List<VipPackageFeature>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.VipPackage))]
        public string Title { get; set; }
        [Display(Name = "Duration", ResourceType = typeof(Resources.Models.VipPackage))]
        public int Duration { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        [Display(Name = "Price", ResourceType = typeof(Resources.Models.VipPackage))]
        public decimal Price { get; set; }
        public virtual ICollection<UserVipPackage> UserVipPackages { get; set; }
        public virtual ICollection<VipPackageFeature> VipPackageFeatures { get; set; }
        internal class configuration : EntityTypeConfiguration<VipPackage>
        {
            public configuration()
            {
                HasRequired(p => p.Product).WithMany(t => t.VipPackages).HasForeignKey(p => p.ProductId);
            }
        }
    }
}
