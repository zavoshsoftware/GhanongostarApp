using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class DiscountCodeRepository : Repository<Models.DiscountCode>, IDiscountCodeRepository
    {
        public DiscountCodeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
