using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SeminarImageRepository : Repository<Models.SeminarImage>, ISeminarImageRepository
    {
        public SeminarImageRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
