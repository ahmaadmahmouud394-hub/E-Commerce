using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    // Used for creating a new role
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Description { get; set; } = string.Empty;

        // Input: Array of Permission IDs (e.g., [1, 4, 7])
        public List<int> PermissionIds { get; set; } = new List<int>();
    }

    // Used for updating an existing role
    public class UpdateRoleDto : CreateRoleDto
    {
        [Required]
        public int Id { get; set; }
    }

    // Used for displaying role details in the API response
    public class RoleResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>(); // List of Permission Names
    }
}