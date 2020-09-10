using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BlogCategoryRepository : Repository<Models.BlogCategory>, IBlogCategoryRepository
    {
        public BlogCategoryRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
