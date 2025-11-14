using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        public int PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission? Permission { get; set; }
    }
}
