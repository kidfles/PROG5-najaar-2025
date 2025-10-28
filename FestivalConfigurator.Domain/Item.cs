using System.ComponentModel.DataAnnotations;

namespace FestivalConfigurator.Domain;

public sealed class Item
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    [Display(Name = "Naam")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Type")]
    public ItemType ItemType { get; set; }

    [Display(Name = "Prijs")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
}


