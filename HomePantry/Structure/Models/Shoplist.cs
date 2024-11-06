using System;
using System.Collections.Generic;

namespace HomePantry.Models
{
    public class Shoplist : IDisplayContainers
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string ShoplistName { get; set; } = null!;

        public DateTime? DataUtworzenia { get; set; }

        public DateTime? DataAktualizacji { get; set; }

        public string? Opis { get; set; }

        public virtual ICollection<ProductsInShoplist> ProductsInShoplists { get; set; } = new List<ProductsInShoplist>();

        public virtual User? User { get; set; }

        public string DisplayName => ShoplistName; 
    }
}
