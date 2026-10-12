using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemById;

public class GetInventoryItemByIdQueryHandler
	: IRequestHandler<GetInventoryItemByIdQuery, ErrorOr<InventoryItemDto>>
{
	private readonly IAppDbContext _context;
	public GetInventoryItemByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<InventoryItemDto>> Handle(GetInventoryItemByIdQuery request, CancellationToken ct)
	{
		var item = await _context.InventoryItems.AsNoTracking()
			.Include(i => i.Product)
			.Include(i => i.ProductVariant)
			.Include(i => i.Warehouse)
			.Where(i => i.Id == request.Id && !i.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (item is null) return Error.NotFound("InventoryItem.NotFound", "موجودی یافت نشد.");
		return item.Adapt<InventoryItemDto>();
	}
}