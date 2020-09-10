using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ZarinpallAuthorityRepository : Repository<Models.ZarinpallAuthority>, IZarinpallAuthorityRepository
    {
        public ZarinpallAuthorityRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
