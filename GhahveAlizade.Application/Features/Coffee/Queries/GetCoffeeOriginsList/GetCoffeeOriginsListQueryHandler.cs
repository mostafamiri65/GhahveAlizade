using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeOriginsList;

public class GetCoffeeOriginsListQueryHandler
    : IRequestHandler<GetCoffeeOriginsListQuery, ErrorOr<PaginatedList<CoffeeOriginListItemDto>>>
{
    private readonly IAppDbContext _context;

    public GetCoffeeOriginsListQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<PaginatedList<CoffeeOriginListItemDto>>> Handle(
        GetCoffeeOriginsListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.CoffeeOrigins
            .AsNoTracking()
            .Where(o => !o.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(o =>
                o.Name.Contains(search) ||
                o.Slug.Contains(search) ||
                (o.CountryCode != null && o.CountryCode.Contains(search)));
        }

        if (request.IsActive.HasValue)
            query = query.Where(o => o.IsActive == request.IsActive.Value);

        query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(o => o.Name),
            ("name", _) => query.OrderBy(o => o.Name),
            ("createdat", "desc") => query.OrderByDescending(o => o.CreatedAtUtc),
            ("createdat", _) => query.OrderBy(o => o.CreatedAtUtc),
            _ => query.OrderBy(o => o.DisplayOrder).ThenBy(o => o.Name)
        };

        var projectedQuery = query.ProjectToType<CoffeeOriginListItemDto>();

        return await PaginatedList<CoffeeOriginListItemDto>.CreateAsync(
            projectedQuery, request.PageNumber, request.PageSize, cancellationToken);
    }
}