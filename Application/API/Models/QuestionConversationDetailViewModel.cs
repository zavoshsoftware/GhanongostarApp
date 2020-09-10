using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class QuestionConversationDetailViewModel : BaseViewModel
    {
        public QuestionConversationDetailMasterViewModel Result { get; set; }
    }

    public class QuestionConversationDetailMasterViewModel
    {
        public string Status { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<QuestionConversationDetailItemViewModel> Conversations { get; set; }
    }
    public class QuestionConversationDetailItemViewModel
    {
        public string Order { get; set; }
        public string FullName { get; set; }
        public string Body { get; set; }
        public string CreationDate { get; set; }
    }
}