using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ProductTypeRepository : Repository<Models.ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }
    }
}
