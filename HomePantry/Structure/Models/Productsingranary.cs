using System.ComponentModel.DataAnnotations;

namespace HomePantry.Models
{
    public class ProductsInGranary
    {
        public int Id { get; set; }

        [Required]
        public int GranaryId { get; set; }

        [Required]
        public string ProductName { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        public string? Description { get; set; }

        public virtual Granary? Granary { get; set; }
    }
}