using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Marketing;

public class Campaign : BaseEntity
{
	public string Name { get; set; } = default!;
	public string? Description { get; set; }
	public string? Code { get; set; }

	public string CampaignType { get; set; } = default!; // Email, SMS, Popup, Social
	public string? TargetAudienceJson { get; set; }
	public string? ContentJson { get; set; }

	public DateTime? StartsAtUtc { get; set; }
	public DateTime? EndsAtUtc { get; set; }

	public bool IsActive { get; set; } = true;

	public int SentCount { get; set; }
	public int OpenCount { get; set; }
	public int ClickCount { get; set; }
	public int ConversionCount { get; set; }
}