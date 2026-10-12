using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributesList;

public class GetAttributesListQueryHandler
    : IRequestHandler<GetAttributesListQuery, ErrorOr<PaginatedList<AttributeListItemDto>>>
{
    private readonly IAppDbContext _context;

    public GetAttributesListQueryHandler(IAppDbContext context) => _context = context;

    public async Task<ErrorOr<PaginatedList<AttributeListItemDto>>> Handle(
        GetAttributesListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Attributes
            .AsNoTracking()
            .Where(a => !a.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(a =>
                a.Name.Contains(search) || a.Slug.Contains(search));
        }

        if (request.Type.HasValue)
            query = query.Where(a => a.Type == request.Type.Value);

        if (request.IsFilterable.HasValue)
            query = query.Where(a => a.IsFilterable == request.IsFilterable.Value);

        if (request.IsVariant.HasValue)
            query = query.Where(a => a.IsVariant == request.IsVariant.Value);

        query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(a => a.Name),
            ("name", _) => query.OrderBy(a => a.Name),
            ("createdat", "desc") => query.OrderByDescending(a => a.CreatedAtUtc),
            ("createdat", _) => query.OrderBy(a => a.CreatedAtUtc),
            ("displayorder", "desc") => query.OrderByDescending(a => a.DisplayOrder),
            _ => query.OrderBy(a => a.DisplayOrder).ThenBy(a => a.Name)
        };

        var projectedQuery = query.ProjectToType<AttributeListItemDto>();

        return await PaginatedList<AttributeListItemDto>.CreateAsync(
            projectedQuery, request.PageNumber, request.PageSize, cancellationToken);
    }
}