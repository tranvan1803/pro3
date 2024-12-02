using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class RegisterViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Display(Name = "Avatar")]
        public IFormFile? Avatar { get; set; }
        public bool IsAdmin { get; set; }
    }

}
