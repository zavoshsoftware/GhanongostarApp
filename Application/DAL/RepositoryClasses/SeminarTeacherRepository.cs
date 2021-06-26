using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class SeminarTeacherRepository : Repository<Models.SeminarTeacher>, ISeminarTeacherRepository
    {
        public SeminarTeacherRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
