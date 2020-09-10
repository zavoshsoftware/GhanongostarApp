using System.Collections.Generic;

namespace API.Models
{
    public class QuestionPackageLisViewModel : BaseViewModel
    {
        public QuestionPackageListViewModel Result { get; set; }
    }

    public class QuestionPackageListViewModel
    {
        public string HasPackage { get; set; }
        public string QuestionNumber { get; set; }
        public List<QuestionPackageItemViewModel> QuestionPackages { get; set; }
    }

    public class QuestionPackageItemViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Code { get; set; }
    }
}