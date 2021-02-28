using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConsultantRequestRepository : Repository<Models.ConsultantRequest>, IConsultantRequestRepository
    {
        public ConsultantRequestRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
