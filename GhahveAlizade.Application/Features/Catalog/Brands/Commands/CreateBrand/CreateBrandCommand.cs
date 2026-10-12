using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.CreateBrand;

public record CreateBrandCommand(
    string Name,
    string? Slug,
    string? Description,
    string? LogoUrl,
    string? WebsiteUrl,
    string? CountryOfOrigin,
    int DisplayOrder,
    bool IsActive,
    bool IsFeatured,
    string? MetaTitle,
    string? MetaDescription,
    string? MetaKeywords,
    string? CanonicalUrl,
    string? OgImage) : IRequest<ErrorOr<long>>;