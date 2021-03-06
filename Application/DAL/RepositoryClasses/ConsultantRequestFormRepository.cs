using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConsultantRequestFormRepository : Repository<Models.ConsultantRequestForm>, IConsultantRequestFormRepository
    {
        public ConsultantRequestFormRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
