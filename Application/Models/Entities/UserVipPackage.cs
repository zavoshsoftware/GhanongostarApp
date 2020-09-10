using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class UserVipPackage:BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid VipPackegeId { get; set; }
         public VipPackage VipPackage { get; set; }
        public DateTime ExpiredDate { get; set; }

        internal class configuration : EntityTypeConfiguration<UserVipPackage>
        {
            public configuration()
            {
                HasRequired(p => p.VipPackage).WithMany(t => t.UserVipPackages).HasForeignKey(p => p.VipPackegeId);

                HasRequired(p => p.User).WithMany(t => t.UserVipPackages).HasForeignKey(p => p.UserId);
            }
        }
    }
}
