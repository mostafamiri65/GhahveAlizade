using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandsList;

public record GetBrandsListQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    bool? IsActive = null,
    bool? IsFeatured = null,
    string? SortBy = "displayOrder",
    string? SortDir = "asc")
    : IRequest<ErrorOr<PaginatedList<BrandListItemDto>>>;