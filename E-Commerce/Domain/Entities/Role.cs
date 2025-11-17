using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class Role 
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }


        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
    public enum ERoles
    {
        Read,
        Write,
        Delete
    }
    public interface IRoles
    {
        public ERoles Products { get; set; }
        public ERoles Users { get; set; }
        public ERoles Roles { get; set; }
        public ERoles Categories { get; set; }
        public ERoles  Brands{ get; set; }
    }
}
