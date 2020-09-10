using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CourseDetailRepository : Repository<Models.CourseDetail>, ICourseDetailRepository
    {
        public CourseDetailRepository(Models.DatabaseContext databaseContext) : base(databaseContext: databaseContext)
        {
            
        }

    }
}
