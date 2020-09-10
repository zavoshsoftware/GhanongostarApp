using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductDiscountRepository : Repository<Models.ProductDiscount>, IProductDiscountRepository
    {
        public ProductDiscountRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
