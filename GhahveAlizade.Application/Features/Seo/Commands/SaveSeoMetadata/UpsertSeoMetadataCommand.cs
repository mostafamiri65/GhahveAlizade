using ErrorOr;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Commands.SaveSeoMetadata;

public record UpsertSeoMetadataCommand(
	long? Id,
	SeoEntityType EntityType,
	long EntityId,
	string Culture,
	string? MetaTitle,
	string? MetaDescription,
	string? MetaKeywords,
	string? CanonicalUrl,
	string? OgTitle,
	string? OgDescription,
	string? OgImage,
	string? OgType,
	string? TwitterCard,
	string? TwitterTitle,
	string? TwitterDescription,
	string? TwitterImage,
	RobotsDirective Robots,
	string? JsonLdSchema,
	string? FocusKeyword,
	int? SeoScore) : IRequest<ErrorOr<long>>;
