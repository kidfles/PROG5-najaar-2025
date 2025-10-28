using System.ComponentModel.DataAnnotations;

namespace FestivalConfigurator.Domain;

public sealed class Item
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ItemType ItemType { get; set; }

    public decimal Price { get; set; }
}


