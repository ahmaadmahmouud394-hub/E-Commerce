using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
    public class Facility
    {
        [Key]
        public int Id { get; set; }
        public required string Alt { get; set; }
        [Required]
        public required string ImageUrl { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
