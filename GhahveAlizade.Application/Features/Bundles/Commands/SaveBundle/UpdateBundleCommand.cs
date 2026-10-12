using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Commands.SaveBundle;

public record UpdateBundleCommand(
	long Id, string Name, string? Slug, string? Description, string? ShortDescription,
	string? ImageUrl, decimal BundlePrice, decimal? CompareAtPrice, decimal? TotalItemsPrice,
	bool IsActive, bool IsFeatured, DateTime? StartsAtUtc, DateTime? ExpiresAtUtc,
	string? MetaTitle, string? MetaDescription, string? CanonicalUrl, string? OgImage,
	List<BundleItemInput> Items) : IRequest<ErrorOr<Success>>;
