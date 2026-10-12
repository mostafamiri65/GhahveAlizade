using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class Banner : BaseEntity
{
	public string Title { get; set; } = default!;
	public string? Subtitle { get; set; }
	public string? Description { get; set; }

	public string ImageUrl { get; set; } = default!;
	public string? MobileImageUrl { get; set; }
	public string? AltText { get; set; }

	public string? LinkUrl { get; set; }
	public string? ButtonText { get; set; }
	public string? Target { get; set; } // _self, _blank

	public DateTime? StartsAtUtc { get; set; }
	public DateTime? ExpiresAtUtc { get; set; }

	public bool IsActive { get; set; } = true;
	public int DisplayOrder { get; set; }
	public int ClickCount { get; set; }
	public int ViewCount { get; set; }

	public ICollection<BannerPlacement> Placements { get; set; } = new List<BannerPlacement>();
}