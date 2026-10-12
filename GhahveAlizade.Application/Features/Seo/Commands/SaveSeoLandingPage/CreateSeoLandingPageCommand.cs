using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoLandingPage;

public record CreateSeoLandingPageCommand(
	string Title, string? Slug, string? H1, string? Content,
	string? FeaturedImageUrl, string? FilterCriteriaJson,
	bool IsPublished,
	string? MetaTitle, string? MetaDescription, string? MetaKeywords,
	string? CanonicalUrl, string? OgImage, string? JsonLdSchema)
	: IRequest<ErrorOr<long>>;
