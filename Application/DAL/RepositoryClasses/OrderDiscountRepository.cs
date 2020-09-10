using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class OrderDiscountRepository : Repository<Models.OrderDiscount>, IOrderDiscountRepository
    {
        public OrderDiscountRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
