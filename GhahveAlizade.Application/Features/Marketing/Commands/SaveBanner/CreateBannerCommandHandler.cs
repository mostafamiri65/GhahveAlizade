using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Marketing;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public class CreateBannerCommandHandler
	: IRequestHandler<CreateBannerCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateBannerCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateBannerCommand request, CancellationToken ct)
	{
		var banner = new Banner
		{
			Title = request.Title.Trim(),
			Subtitle = request.Subtitle?.Trim(),
			Description = request.Description?.Trim(),
			ImageUrl = request.ImageUrl.Trim(),
			MobileImageUrl = request.MobileImageUrl?.Trim(),
			AltText = request.AltText?.Trim(),
			LinkUrl = request.LinkUrl?.Trim(),
			ButtonText = request.ButtonText?.Trim(),
			Target = request.Target?.Trim() ?? "_self",
			StartsAtUtc = request.StartsAtUtc,
			ExpiresAtUtc = request.ExpiresAtUtc,
			IsActive = request.IsActive,
			DisplayOrder = request.DisplayOrder
		};

		foreach (var p in request.Placements)
		{
			banner.Placements.Add(new BannerPlacement
			{
				Placement = (BannerPlacementType)p.Placement,
				DisplayOrder = p.DisplayOrder,
				IsActive = p.IsActive
			});
		}

		_context.Banners.Add(banner);
		await _context.SaveChangesAsync(ct);
		return banner.Id;
	}
}
