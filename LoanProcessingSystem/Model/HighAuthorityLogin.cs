using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Model
{
    public class HighAuthorityLogin
    {

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName is required")]
        public string EmailId { get; set; }
        [Display(Name = "Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}