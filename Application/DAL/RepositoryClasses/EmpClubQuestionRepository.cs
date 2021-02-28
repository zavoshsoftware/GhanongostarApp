using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmpClubQuestionRepository : Repository<Models.EmpClubQuestion>, IEmpClubQuestionRepository
    {
        public EmpClubQuestionRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
