using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Email now")]
        public string EmailNow { get; set; }
        [Display(Name = "Password now")]
        public string PasswordNow { get; set; }
        [Display(Name = "Password new")]
        [Required(ErrorMessage = "Please import password new")]
        [MinLength(5, ErrorMessage = "Please import min 5 characters")]
        public string PasswordNew { get; set; }
        [Display(Name = "Password confirm")]
        [Required(ErrorMessage = "Please import password confirm")]
        [MinLength(5, ErrorMessage = "Please import min 5 characters")]
        public string ConfirmPassword { get; set; }
    }
}
