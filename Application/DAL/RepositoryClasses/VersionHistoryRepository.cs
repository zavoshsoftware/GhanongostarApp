using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DAL
{
   public  class VersionHistoryRepository : Repository<Models.VersionHistory>, IVersionHistoryRepository
    {
        public VersionHistoryRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
