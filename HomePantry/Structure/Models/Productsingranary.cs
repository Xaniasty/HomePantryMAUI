using HomePantry.Structure.Models.HomePantry.Models;
using System.ComponentModel.DataAnnotations;

namespace HomePantry.Models
{
    public class ProductsInGranary : IProduct
    {
        public int ProductId { get; set; }
        public int GranaryId { get; set; }
        public string ProductName { get; set; } = null!;
        public int? Quantity { get; set; }
        public decimal? Weight { get; set; } = 0.01m;
        public string? Description { get; set; }
        public bool IsLiquid { get; set; } = false;
        public decimal? Cena { get; set; }
        public string? Rodzaj { get; set; }
        public bool InPackage { get; set; } = false;
        public DateOnly? DataZakupu { get; set; }
        public DateOnly? DataWaznosci { get; set; }
    }
}