using GhahveAlizade.Domain.Common;
using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Domain.Marketing;

public class BannerPlacement : BaseEntity
{
	public long BannerId { get; set; }
	public Banner Banner { get; set; } = default!;

	public BannerPlacementType Placement { get; set; }
	public int DisplayOrder { get; set; }
	public bool IsActive { get; set; } = true;
}