using HomePantry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomePantry.Models
{
    public class Granary
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string GranaryName { get; set; } = null!;

        public DateTime? DataUtworzenia { get; set; }

        public DateTime? DataAktualizacji { get; set; }

        public string? Opis { get; set; }
        public virtual ICollection<ProductsInGranary> ProductsInGranaries { get; set; } = new List<ProductsInGranary>();

        public virtual User? User { get; set; }
    }
}