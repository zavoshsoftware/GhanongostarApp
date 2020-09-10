using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class QuestionConversationListViewModel : BaseViewModel
    {
        public List<QuestionConversationItemViewModel> Result { get; set; }
    }

    public class QuestionConversationItemViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public string CreationDate { get; set; }
    }
}