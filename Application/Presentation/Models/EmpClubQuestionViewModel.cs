using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class EmpClubQuestionViewModel
    {
        public EmpClubQuestion UserQuestion { get; set; }
        public List<EmpClubQuestion> UserQuestions { get; set; }
    }
}