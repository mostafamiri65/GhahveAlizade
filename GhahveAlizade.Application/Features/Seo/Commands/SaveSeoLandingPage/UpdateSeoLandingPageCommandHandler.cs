using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public class UpdateSeoLandingPageCommandHandler
	: IRequestHandler<UpdateSeoLandingPageCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public UpdateSeoLandingPageCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(UpdateSeoLandingPageCommand request, CancellationToken ct)
	{
		var page = await _context.SeoLandingPages
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (page is null) return Error.NotFound("LandingPage.NotFound", "صفحه یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Title);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.SeoLandingPages.AnyAsync(
			p => p.Slug == slug && p.Id != page.Id && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var wasPublished = page.IsPublished;

		page.Title = request.Title.Trim();
		page.Slug = slug;
		page.H1 = request.H1?.Trim();
		page.Content = request.Content;
		page.FeaturedImageUrl = request.FeaturedImageUrl?.Trim();
		page.FilterCriteriaJson = request.FilterCriteriaJson;
		page.IsPublished = request.IsPublished;
		page.MetaTitle = request.MetaTitle?.Trim();
		page.MetaDescription = request.MetaDescription?.Trim();
		page.MetaKeywords = request.MetaKeywords?.Trim();
		page.CanonicalUrl = request.CanonicalUrl?.Trim();
		page.OgImage = request.OgImage?.Trim();
		page.JsonLdSchema = request.JsonLdSchema;

		if (request.IsPublished && !wasPublished)
			page.PublishedAtUtc = _dateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
