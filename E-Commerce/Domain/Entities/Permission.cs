using System.ComponentModel.DataAnnotations;

namespace E_Commerce.Domain.Entities
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description {  get; set; }

        //public virtual ICollection<RolePermission> UserPermissions { get; set; }
    }
}
