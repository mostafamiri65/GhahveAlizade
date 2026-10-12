using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler
    : IRequestHandler<CreateBrandCommand, ErrorOr<long>>
{
    private readonly IAppDbContext _context;

    public CreateBrandCommandHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<long>> Handle(
        CreateBrandCommand request,
        CancellationToken cancellationToken)
    {
        var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
        var slug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

        var brand = new Brand
        {
            Name = request.Name.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            LogoUrl = request.LogoUrl?.Trim(),
            WebsiteUrl = request.WebsiteUrl?.Trim(),
            CountryOfOrigin = request.CountryOfOrigin?.Trim(),
            DisplayOrder = request.DisplayOrder,
            IsActive = request.IsActive,
            IsFeatured = request.IsFeatured,
            MetaTitle = request.MetaTitle?.Trim(),
            MetaDescription = request.MetaDescription?.Trim(),
            MetaKeywords = request.MetaKeywords?.Trim(),
            CanonicalUrl = request.CanonicalUrl?.Trim(),
            OgImage = request.OgImage?.Trim()
        };

        _context.Brands.Add(brand);
        await _context.SaveChangesAsync(cancellationToken);

        return brand.Id;
    }

    private async Task<string> EnsureUniqueSlugAsync(
        string baseSlug, long? excludeId, CancellationToken ct)
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