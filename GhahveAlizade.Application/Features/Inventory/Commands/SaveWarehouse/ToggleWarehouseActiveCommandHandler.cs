using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public class ToggleWarehouseActiveCommandHandler
	: IRequestHandler<ToggleWarehouseActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleWarehouseActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleWarehouseActiveCommand request, CancellationToken ct)
	{
		var w = await _context.Warehouses
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (w is null) return Error.NotFound("Warehouse.NotFound", "انبار یافت نشد.");

		w.IsActive = !w.IsActive;
		await _context.SaveChangesAsync(ct);
		return w.IsActive;
	}
}