using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SiteBlogCategoryRepository : Repository<Models.SiteBlogCategory>, ISiteBlogCategoryRepository
    {
        public SiteBlogCategoryRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
