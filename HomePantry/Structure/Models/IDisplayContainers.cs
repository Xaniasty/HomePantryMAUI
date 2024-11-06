using System;

namespace HomePantry.Models
{
    public interface IDisplayContainers
    {
        int Id { get; }
        int UserId { get; }
        string DisplayName { get; }
        DateTime? DataUtworzenia { get; }
        DateTime? DataAktualizacji { get; }
        string? Opis { get; }
    }
}