using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BlogRepository : Repository<Models.Blog>, IBlogRepository
    {
        public BlogRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
