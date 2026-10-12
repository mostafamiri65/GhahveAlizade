using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.SEO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public class CreateSeoLandingPageCommandHandler
	: IRequestHandler<CreateSeoLandingPageCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public CreateSeoLandingPageCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<long>> Handle(CreateSeoLandingPageCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Title);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.SeoLandingPages.AnyAsync(p => p.Slug == slug && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var page = new SeoLandingPage
		{
			Title = request.Title.Trim(),
			Slug = slug,
			H1 = request.H1?.Trim(),
			Content = request.Content,
			FeaturedImageUrl = request.FeaturedImageUrl?.Trim(),
			FilterCriteriaJson = request.FilterCriteriaJson,
			IsPublished = request.IsPublished,
			PublishedAtUtc = request.IsPublished ? _dateTime.UtcNow : null,
			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			MetaKeywords = request.MetaKeywords?.Trim(),
			CanonicalUrl = request.CanonicalUrl?.Trim(),
			OgImage = request.OgImage?.Trim(),
			JsonLdSchema = request.JsonLdSchema
		};

		_context.SeoLandingPages.Add(page);
		await _context.SaveChangesAsync(ct);
		return page.Id;
	}
}
