using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PageRepository : Repository<Models.Page>, IPageRepository
    {
        public PageRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
        }
    }
}
