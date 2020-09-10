using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SiteBlogImageRepository : Repository<Models.SiteBlogImage>, ISiteBlogImageRepository
    {
        public SiteBlogImageRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
