using System.ComponentModel.DataAnnotations;

namespace FestivalConfigurator.Domain;

public sealed class Package
{
    public int Id { get; set; }

    public int FestivalId { get; set; }
    public Festival Festival { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public ICollection<PackageItem> PackageItems { get; set; } = new List<PackageItem>();
}


