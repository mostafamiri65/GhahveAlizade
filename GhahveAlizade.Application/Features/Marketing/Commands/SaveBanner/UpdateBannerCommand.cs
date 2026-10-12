using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public record UpdateBannerCommand(
	long Id,
	string Title,
	string? Subtitle,
	string? Description,
	string ImageUrl,
	string? MobileImageUrl,
	string? AltText,
	string? LinkUrl,
	string? ButtonText,
	string? Target,
	DateTime? StartsAtUtc,
	DateTime? ExpiresAtUtc,
	bool IsActive,
	int DisplayOrder,
	List<BannerPlacementInput> Placements) : IRequest<ErrorOr<Success>>;
