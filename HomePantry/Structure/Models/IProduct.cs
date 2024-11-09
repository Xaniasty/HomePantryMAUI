using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomePantry.Structure.Models
{
    namespace HomePantry.Models
    {
        public interface IProduct
        {
            string ProductName { get; set; }
            int? Quantity { get; set; }
            decimal? Weight { get; set; }
            string? Description { get; set; }
            bool IsLiquid { get; set; }
            decimal? Cena { get; set; }
            string? Rodzaj { get; set; }
            bool InPackage { get; set; }
            DateOnly? DataZakupu { get; set; }
            DateOnly? DataWaznosci { get; set; }
        }
    }

}
