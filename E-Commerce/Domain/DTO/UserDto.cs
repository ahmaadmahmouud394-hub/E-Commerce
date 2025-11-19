using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class UserDto
    {
        public class CreateUserDto
        {
            [Required(ErrorMessage = "Username is required.")]
            public string UserName { get; set; } = string.Empty;

            [Required(ErrorMessage = "Password is required.")]
            public string Password { get; set; } = string.Empty;

            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            public string Name { get; set; } = string.Empty;

            [Required]
            public string Address { get; set; } = string.Empty;

            [Required]
            public int RoleId { get; set; }

            public DateOnly? BirthDate { get; set; }

            // Image data sent as a byte array (from Base64 decoding by the framework/client)
            public byte[]? Base64Image { get; set; }
        }

        public class UpdateUserDto : CreateUserDto
        {
            [Required]
            public int Id { get; set; }
        }

        // --- Output DTO (Safe response structure) ---

        public class UserResponseDto
        {
            public int Id { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string RoleName { get; set; } = string.Empty;
            public string? AvatarUrl { get; set; }
        }
    }
}