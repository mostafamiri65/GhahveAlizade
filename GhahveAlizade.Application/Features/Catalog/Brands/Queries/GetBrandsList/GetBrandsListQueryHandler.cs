using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandsList;

public class GetBrandsListQueryHandler
    : IRequestHandler<GetBrandsListQuery, ErrorOr<PaginatedList<BrandListItemDto>>>
{
    private readonly IAppDbContext _context;

    public GetBrandsListQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<PaginatedList<BrandListItemDto>>> Handle(
        GetBrandsListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Brands
            .AsNoTracking()
            .Where(b => !b.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(b =>
                b.Name.Contains(search) ||
                b.Slug.Contains(search) ||
                (b.CountryOfOrigin != null && b.CountryOfOrigin.Contains(search)));
        }

        if (request.IsActive.HasValue)
            query = query.Where(b => b.IsActive == request.IsActive.Value);

        if (request.IsFeatured.HasValue)
            query = query.Where(b => b.IsFeatured == request.IsFeatured.Value);

        query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(b => b.Name),
            ("name", _) => query.OrderBy(b => b.Name),
            ("createdat", "desc") => query.OrderByDescending(b => b.CreatedAtUtc),
            ("createdat", _) => query.OrderBy(b => b.CreatedAtUtc),
            ("displayorder", "desc") => query.OrderByDescending(b => b.DisplayOrder),
            _ => query.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
        };

        var projectedQuery = query.ProjectToType<BrandListItemDto>();

        return await PaginatedList<BrandListItemDto>.CreateAsync(
            projectedQuery, request.PageNumber, request.PageSize, cancellationToken);
    }
}