using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public class DeleteWarehouseCommandHandler
	: IRequestHandler<DeleteWarehouseCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteWarehouseCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteWarehouseCommand request, CancellationToken ct)
	{
		var w = await _context.Warehouses
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (w is null) return Error.NotFound("Warehouse.NotFound", "انبار یافت نشد.");

		if (w.IsDefault)
			return Error.Conflict("Warehouse.IsDefault", "انبار پیش‌فرض قابل حذف نیست.");

		var hasItems = await _context.InventoryItems
			.AnyAsync(i => i.WarehouseId == w.Id && !i.IsDeleted, ct);
		if (hasItems)
			return Error.Conflict("Warehouse.HasItems",
				"این انبار دارای موجودی است و قابل حذف نیست. ابتدا موجودی را منتقل کنید.");

		_context.Warehouses.Remove(w);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
