using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce.Domain.Entities
{
    public class Product
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity {  get; set; }
        public bool InWarranty {  get; set; }
        public string WarrantyDescription {  get; set; }
        public DateOnly WarrantyMaxDate { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }

        //public virtual ICollection<Facility> Facilities { get; set; }
        //public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
