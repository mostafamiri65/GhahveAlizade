using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.UpdateBrand;

public class UpdateBrandCommandHandler
    : IRequestHandler<UpdateBrandCommand, ErrorOr<Success>>
{
    private readonly IAppDbContext _context;

    public UpdateBrandCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<Success>> Handle(
        UpdateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var brand = await _context.Brands
            .FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, cancellationToken);

        if (brand is null)
            return Error.NotFound("Brand.NotFound", "برند یافت نشد.");

        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, brand.Id, cancellationToken);

        brand.Name = request.Name.Trim();
        brand.Slug = slug;
        brand.Description = request.Description?.Trim();
        brand.LogoUrl = request.LogoUrl?.Trim();
        brand.WebsiteUrl = request.WebsiteUrl?.Trim();
        brand.CountryOfOrigin = request.CountryOfOrigin?.Trim();
        brand.DisplayOrder = request.DisplayOrder;
        brand.IsActive = request.IsActive;
        brand.IsFeatured = request.IsFeatured;
        brand.MetaTitle = request.MetaTitle?.Trim();
        brand.MetaDescription = request.MetaDescription?.Trim();
        brand.MetaKeywords = request.MetaKeywords?.Trim();
        brand.CanonicalUrl = request.CanonicalUrl?.Trim();
        brand.OgImage = request.OgImage?.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string baseSlug, long excludeId, CancellationToken ct)
    {
        var slug = baseSlug;
        var counter = 1;

        while (await _context.Brands.AnyAsync(
            b => b.Slug == slug && b.Id != excludeId && !b.IsDeleted, ct))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }
}