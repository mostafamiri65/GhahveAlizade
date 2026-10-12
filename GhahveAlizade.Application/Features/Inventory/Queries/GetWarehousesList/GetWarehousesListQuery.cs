using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetWarehousesList;

public record GetWarehousesListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	bool? IsActive = null, string? SortBy = "name", string? SortDir = "asc")
	: IRequest<ErrorOr<PaginatedList<WarehouseListItemDto>>>;