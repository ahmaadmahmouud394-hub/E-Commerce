using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public DateOnly BirthDate { get; set; }
    }
}