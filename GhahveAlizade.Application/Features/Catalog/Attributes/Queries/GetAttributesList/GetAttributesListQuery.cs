using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributesList;

public record GetAttributesListQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null,
    AttributeType? Type = null,
    bool? IsFilterable = null,
    bool? IsVariant = null,
    string? SortBy = "displayOrder",
    string? SortDir = "asc")
    : IRequest<ErrorOr<PaginatedList<AttributeListItemDto>>>;