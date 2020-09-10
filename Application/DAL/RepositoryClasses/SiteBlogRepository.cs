using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SiteBlogRepository : Repository<Models.SiteBlog>, ISiteBlogRepository
    {
        public SiteBlogRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
