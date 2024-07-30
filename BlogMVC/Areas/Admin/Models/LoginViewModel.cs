using MessagePack;
using System.ComponentModel.DataAnnotations;

namespace BlogMVC.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [MaxLength(50)]
        [Required(ErrorMessage ="Please import email")]
        [Display(Name ="Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Please import email")]
        public string Email { get; set; }

        [Display(Name ="PassWord")]
        [Required(ErrorMessage = "Please import password")]
        [MaxLength(30,ErrorMessage = "password lenght 30 characters")]
        public string Password { get; set; }

        public bool KeepMeLogin { get; set; }
    }
}
