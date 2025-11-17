using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace E_Commerce.Models.DTO
{
    public class CreateUserRequestDto
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

        // This now accepts a list of roles
        [Required]
        public List<int> RoleIds { get; set; }
    }
}