using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class FormInstagramLiveRepository : Repository<Models.FormInstagramLive>, IFormInstagramLiveRepository
    {
        public FormInstagramLiveRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
