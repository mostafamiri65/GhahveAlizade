using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemsList;

public class GetInventoryItemsListQueryHandler
	: IRequestHandler<GetInventoryItemsListQuery, ErrorOr<PaginatedList<InventoryItemListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetInventoryItemsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<InventoryItemListItemDto>>> Handle(
		GetInventoryItemsListQuery request, CancellationToken ct)
	{
		var query = _context.InventoryItems
			.AsNoTracking()
			.Include(i => i.Product)
			.Include(i => i.ProductVariant)
			.Include(i => i.Warehouse)
			.Where(i => !i.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(i =>
				i.Product.Name.Contains(s) ||
				(i.Product.Sku != null && i.Product.Sku.Contains(s)));
		}

		if (request.WarehouseId.HasValue)
			query = query.Where(i => i.WarehouseId == request.WarehouseId.Value);

		if (request.ProductId.HasValue)
			query = query.Where(i => i.ProductId == request.ProductId.Value);

		if (request.LowStockOnly)
			query = query.Where(i => i.ReorderLevel.HasValue &&
				(i.Quantity - i.ReservedQuantity) <= i.ReorderLevel.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("quantity", "asc") => query.OrderBy(i => i.Quantity),
			("quantity", _) => query.OrderByDescending(i => i.Quantity),
			("productname", "asc") => query.OrderBy(i => i.Product.Name),
			("productname", _) => query.OrderByDescending(i => i.Product.Name),
			("updatedat", "asc") => query.OrderBy(i => i.UpdatedAtUtc),
			_ => query.OrderByDescending(i => i.UpdatedAtUtc)
		};

		return await PaginatedList<InventoryItemListItemDto>.CreateAsync(
			query.ProjectToType<InventoryItemListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}