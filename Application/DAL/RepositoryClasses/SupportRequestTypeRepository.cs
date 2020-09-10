using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SupportRequestTypeRepository : Repository<Models.SupportRequestType>, ISupportRequestTypeRepository
    {
        public SupportRequestTypeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
