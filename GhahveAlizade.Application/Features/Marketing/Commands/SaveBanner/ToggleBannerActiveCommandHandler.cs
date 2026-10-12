using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Marketing.Commands.SaveBanner;

public class ToggleBannerActiveCommandHandler
	: IRequestHandler<ToggleBannerActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleBannerActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleBannerActiveCommand request, CancellationToken ct)
	{
		var banner = await _context.Banners
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (banner is null)
			return Error.NotFound("Banner.NotFound", "بنر یافت نشد.");

		banner.IsActive = !banner.IsActive;
		await _context.SaveChangesAsync(ct);
		return banner.IsActive;
	}
}