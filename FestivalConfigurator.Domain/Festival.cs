using System.ComponentModel.DataAnnotations;

namespace FestivalConfigurator.Domain;

public sealed class Festival
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    [Display(Name = "Naam")]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    [Display(Name = "Plaats")]
    public string Place { get; set; } = string.Empty;

    [MaxLength(500)]
    [Display(Name = "Logo")]
    public string? Logo { get; set; }

    [MaxLength(2000)]
    [Display(Name = "Beschrijving")]
    public string? Description { get; set; }

    [Display(Name = "Basisprijs")]
    public decimal BasicPrice { get; set; }

    [Display(Name = "Startdatum")]
    public DateOnly StartDate { get; set; }
    [Display(Name = "Einddatum")]
    public DateOnly EndDate   { get; set; }

    public ICollection<Package> Packages { get; set; } = new List<Package>();
}


