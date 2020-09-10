using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
  public  class TextRepository : Repository<Models.Text>, ITextRepository
    {
        public TextRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {

        }
    }
}
