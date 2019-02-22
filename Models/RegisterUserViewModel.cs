using System.ComponentModel.DataAnnotations;

namespace CLGE_CORE.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }
          
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
      
        public bool Configure { get; set; }
    }
}