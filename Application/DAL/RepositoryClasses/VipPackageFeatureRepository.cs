using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class VipPackageFeatureRepository : Repository<Models.VipPackageFeature>, IVipPackageFeatureRepository
    {
        public VipPackageFeatureRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
