using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetLowStockAlerts;

public class GetLowStockAlertsQueryHandler
	: IRequestHandler<GetLowStockAlertsQuery, ErrorOr<List<LowStockAlertDto>>>
{
	private readonly IAppDbContext _context;

	public GetLowStockAlertsQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<LowStockAlertDto>>> Handle(
		GetLowStockAlertsQuery request, CancellationToken ct)
	{
		var result = await _context.InventoryItems
			.AsNoTracking()
			.Include(i => i.Product)
			.Include(i => i.ProductVariant)
			.Include(i => i.Warehouse)
			.Where(i => !i.IsDeleted &&
					   i.ReorderLevel.HasValue &&
					   (i.Quantity - i.ReservedQuantity) <= i.ReorderLevel.Value)
			.OrderBy(i => i.Quantity - i.ReservedQuantity)
			.Take(request.Count)
			.Select(i => new LowStockAlertDto
			{
				InventoryItemId = i.Id,
				ProductName = i.Product.Name,
				VariantName = i.ProductVariant != null ? i.ProductVariant.Name : null,
				WarehouseName = i.Warehouse.Name,
				AvailableQuantity = i.Quantity - i.ReservedQuantity,
				ReorderLevel = i.ReorderLevel ?? 0
			})
			.ToListAsync(ct);

		return result;
	}
}