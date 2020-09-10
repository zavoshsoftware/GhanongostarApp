using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ActivationCodeRepository : Repository<Models.ActivationCode>, IActivationCodeRepository
    {
        public ActivationCodeRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
