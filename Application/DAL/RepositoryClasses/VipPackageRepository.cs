using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class VipPackageRepository : Repository<Models.VipPackage>, IVipPackageRepository
    {
        public VipPackageRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
