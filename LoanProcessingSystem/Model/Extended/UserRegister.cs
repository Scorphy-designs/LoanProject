using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LoanProcessingSystem.Model
{
    [MetadataType(typeof(UserRegisterMetadata))]
    public partial class UserRegister
    {
        public string ConfirmPassword { get; set; }
    }
    public class UserRegisterMetadata
    {
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Require")]
        public string FullName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id require")]
        [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$", ErrorMessage = "Not in Valid Format")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; }

        [Display(Name = "Gender")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select")]
        public string Gender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password require")]
        //[RegularExpression("/^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])\\w{6,}$/",ErrorMessage ="It should contain number lowercase and uppercase letters")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 char")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Should match")]
        public string ConfirmPassword { get; set; }
    }
}