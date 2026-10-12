using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemsList;

public record GetInventoryItemsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? WarehouseId = null,
	long? ProductId = null,
	bool LowStockOnly = false,
	string? SortBy = "updatedAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<InventoryItemListItemDto>>>;