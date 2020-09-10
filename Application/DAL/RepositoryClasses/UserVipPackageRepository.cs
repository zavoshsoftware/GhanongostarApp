using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserVipPackageRepository : Repository<Models.UserVipPackage>, IUserVipPackageRepository
    {
        public UserVipPackageRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
