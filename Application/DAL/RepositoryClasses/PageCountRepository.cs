using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PageCountRepository : Repository<Models.PageCount>, IPageCountRepository
    {
        public PageCountRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
