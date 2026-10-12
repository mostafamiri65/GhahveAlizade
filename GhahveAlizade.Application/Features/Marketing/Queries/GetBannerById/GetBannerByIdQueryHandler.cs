using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannerById;

public class GetBannerByIdQueryHandler
	: IRequestHandler<GetBannerByIdQuery, ErrorOr<BannerDto>>
{
	private readonly IAppDbContext _context;

	public GetBannerByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BannerDto>> Handle(GetBannerByIdQuery request, CancellationToken ct)
	{
		var banner = await _context.Banners.AsNoTracking()
			.Include(b => b.Placements)
			.Where(b => b.Id == request.Id && !b.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (banner is null)
			return Error.NotFound("Banner.NotFound", "بنر یافت نشد.");

		return banner.Adapt<BannerDto>();
	}
}