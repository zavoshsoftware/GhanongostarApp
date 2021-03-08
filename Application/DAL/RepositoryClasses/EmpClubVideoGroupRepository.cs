using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmpClubVideoGroupRepository : Repository<Models.EmpClubVideoGroup>, IEmpClubVideoGroupRepository
    {
        public EmpClubVideoGroupRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
