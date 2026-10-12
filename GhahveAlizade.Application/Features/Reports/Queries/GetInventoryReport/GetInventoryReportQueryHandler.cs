using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetInventoryReport;

public class GetInventoryReportQueryHandler
	: IRequestHandler<GetInventoryReportQuery, ErrorOr<InventoryReportDto>>
{
	private readonly IAppDbContext _context;
	private readonly IMediator _mediator;

	public GetInventoryReportQueryHandler(IAppDbContext context, IMediator mediator)
	{
		_context = context;
		_mediator = mediator;
	}

	public async Task<ErrorOr<InventoryReportDto>> Handle(
		GetInventoryReportQuery request, CancellationToken ct)
	{
		var items = await _context.InventoryItems
			.AsNoTracking()
			.Include(i => i.Product)
			.Include(i => i.Warehouse)
			.Where(i => !i.IsDeleted)
			.ToListAsync(ct);

		var dto = new InventoryReportDto
		{
			TotalItems = items.Count,
			TotalValue = items.Sum(i => i.Quantity * (i.Product.CostPrice ?? 0)),
			LowStockCount = items.Count(i => i.ReorderLevel.HasValue &&
											 (i.Quantity - i.ReservedQuantity) <= i.ReorderLevel.Value &&
											 i.Quantity > 0),
			OutOfStockCount = items.Count(i => i.Quantity - i.ReservedQuantity <= 0)
		};

		// Stock by warehouse
		var byWarehouse = items
			.GroupBy(i => i.Warehouse.Name)
			.ToDictionary(
				g => g.Key,
				g => g.Sum(i => i.Quantity));

		dto.StockByWarehouse = byWarehouse;

		// Low stock list
		var lowStock = await _mediator.Send(new GetLowStockAlerts.GetLowStockAlertsQuery(20), ct);
		if (!lowStock.IsError) dto.LowStockItems = lowStock.Value;

		return dto;
	}
}