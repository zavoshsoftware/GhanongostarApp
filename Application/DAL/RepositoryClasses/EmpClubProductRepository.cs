using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmpClubProductRepository : Repository<Models.EmpClubProduct>, IEmpClubProductRepository
    {
        public EmpClubProductRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
