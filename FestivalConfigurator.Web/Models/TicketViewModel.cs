using System.ComponentModel.DataAnnotations;
using FestivalConfigurator.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FestivalConfigurator.Web.Models;

public sealed class TicketViewModel
{
    public int PackageId { get; set; }
    public string PackageName { get; set; } = string.Empty;

    public int FestivalId { get; set; }
    public string FestivalName { get; set; } = string.Empty;
    public string FestivalPlace { get; set; } = string.Empty;
    public string? FestivalLogo { get; set; }
    public string? FestivalDescription { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal BasicPrice { get; set; }

    public decimal PackageTotal { get; set; }

    public List<TicketPanelViewModel> Panels { get; set; } = new();
}

public sealed class TicketPanelViewModel
{
    public ItemType Type { get; set; }

    public string TypeDisplay => Type switch
    {
        ItemType.Food_and_Drinks => "Eten & Drinken",
        ItemType.VIPAccess => "VIP Toegang",
        _ => Type.ToString()
    };

    public int? SelectedItemId { get; set; }
    public string? SelectedItemName { get; set; }
    public decimal? UnitPrice { get; set; }

    [Range(0, 1000)]
    public int Quantity { get; set; }

    public decimal LineTotal => (UnitPrice ?? 0m) * Quantity;

    public IEnumerable<SelectListItem> Options { get; set; } = Enumerable.Empty<SelectListItem>();

    public string? IconPath { get; set; }
}

public sealed class TicketPostModel
{
    [Required]
    public int PackageId { get; set; }

    [Required]
    public ItemType Type { get; set; }

    public int? ItemId { get; set; }

    [Range(0, 1000)]
    public int Quantity { get; set; }

    public string Command { get; set; } = "select";
}


