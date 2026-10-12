using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Marketing;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public class UpdateBannerCommandHandler
	: IRequestHandler<UpdateBannerCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateBannerCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateBannerCommand request, CancellationToken ct)
	{
		var banner = await _context.Banners
			.Include(b => b.Placements)
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (banner is null)
			return Error.NotFound("Banner.NotFound", "بنر یافت نشد.");

		banner.Title = request.Title.Trim();
		banner.Subtitle = request.Subtitle?.Trim();
		banner.Description = request.Description?.Trim();
		banner.ImageUrl = request.ImageUrl.Trim();
		banner.MobileImageUrl = request.MobileImageUrl?.Trim();
		banner.AltText = request.AltText?.Trim();
		banner.LinkUrl = request.LinkUrl?.Trim();
		banner.ButtonText = request.ButtonText?.Trim();
		banner.Target = request.Target?.Trim() ?? "_self";
		banner.StartsAtUtc = request.StartsAtUtc;
		banner.ExpiresAtUtc = request.ExpiresAtUtc;
		banner.IsActive = request.IsActive;
		banner.DisplayOrder = request.DisplayOrder;

		// Placements: حذف قبلی‌ها، اضافه‌ی جدیدها
		foreach (var p in banner.Placements.ToList())
			_context.BannerPlacements.Remove(p);

		foreach (var p in request.Placements)
		{
			banner.Placements.Add(new BannerPlacement
			{
				BannerId = banner.Id,
				Placement = (BannerPlacementType)p.Placement,
				DisplayOrder = p.DisplayOrder,
				IsActive = p.IsActive
			});
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
