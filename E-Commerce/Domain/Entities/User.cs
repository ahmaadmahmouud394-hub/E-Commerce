using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public  string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string UserName { get; set; } = string.Empty;
        public DateOnly BirthDate {  get; set; }
        public string? AvatarUrl { get; set; }

        //public int RoleId { get; set; }
        //[ForeignKey("RoleId")]
        //public virtual Role? Role { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
