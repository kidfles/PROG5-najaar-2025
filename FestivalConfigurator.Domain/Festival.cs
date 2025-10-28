using System.ComponentModel.DataAnnotations;

namespace FestivalConfigurator.Domain;

public sealed class Festival
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Place { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Logo { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public decimal BasicPrice { get; set; }

    public DateOnly StartDate { get; set; }
    public DateOnly EndDate   { get; set; }

    public ICollection<Package> Packages { get; set; } = new List<Package>();
}


