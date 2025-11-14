using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Address { get; set; }
        public required string UserName { get; set; }
        public DateOnly BirthDate {  get; set; }
        public string? AvatarUrl { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
