
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ConsultantFormViewModel : _BaseViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Company { get; set; }
        [Required]
        public string ContactNumber { get; set; }
        public string Message { get; set; }

        public string ActionType { get; set; }
        [Required]
        public string EmployeeQuantity { get; set; }
    }
}