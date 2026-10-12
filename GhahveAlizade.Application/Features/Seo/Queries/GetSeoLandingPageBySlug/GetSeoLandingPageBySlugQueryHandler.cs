using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPageBySlug;

public class GetSeoLandingPageBySlugQueryHandler
	: IRequestHandler<GetSeoLandingPageBySlugQuery, ErrorOr<SeoLandingPageDto>>
{
	private readonly IAppDbContext _context;

	public GetSeoLandingPageBySlugQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<SeoLandingPageDto>> Handle(
		GetSeoLandingPageBySlugQuery request,
		CancellationToken cancellationToken)
	{
		var page = await _context.SeoLandingPages
			.AsNoTracking()
			.Where(l => l.Slug == request.Slug && !l.IsDeleted && l.IsPublished)
			.ProjectToType<SeoLandingPageDto>()
			.FirstOrDefaultAsync(cancellationToken);

		if (page is null)
			return Error.NotFound("LandingPage.NotFound", "صفحه یافت نشد.");

		return page;
	}
}