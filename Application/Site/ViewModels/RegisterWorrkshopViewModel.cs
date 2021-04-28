using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class RegisterWorrkshopViewModel : _BaseViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; } 
        [Required]
        public string ContactNumber { get; set; }
        [Required]
        public string InstagramId { get; set; }
    }
}