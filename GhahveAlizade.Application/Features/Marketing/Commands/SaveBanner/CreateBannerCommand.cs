using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public record CreateBannerCommand(
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
	List<BannerPlacementInput> Placements) : IRequest<ErrorOr<long>>;
