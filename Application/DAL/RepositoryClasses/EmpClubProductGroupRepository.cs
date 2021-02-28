using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmpClubProductGroupRepository : Repository<Models.EmpClubProductGroup>, IEmpClubProductGroupRepository
    {
        public EmpClubProductGroupRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
