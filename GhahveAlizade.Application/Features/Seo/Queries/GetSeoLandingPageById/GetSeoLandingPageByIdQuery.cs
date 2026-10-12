using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPageById;

public record GetSeoLandingPageByIdQuery(long Id) : IRequest<ErrorOr<SeoLandingPageDto>>;

public class GetSeoLandingPageByIdQueryHandler
	: IRequestHandler<GetSeoLandingPageByIdQuery, ErrorOr<SeoLandingPageDto>>
{
	private readonly IAppDbContext _context;
	public GetSeoLandingPageByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<SeoLandingPageDto>> Handle(GetSeoLandingPageByIdQuery request, CancellationToken ct)
	{
		var p = await _context.SeoLandingPages.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<SeoLandingPageDto>()
			.FirstOrDefaultAsync(ct);

		if (p is null) return Error.NotFound("LandingPage.NotFound", "صفحه یافت نشد.");
		return p;
	}
}