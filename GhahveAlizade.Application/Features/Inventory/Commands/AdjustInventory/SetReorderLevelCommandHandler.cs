using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public class SetReorderLevelCommandHandler
	: IRequestHandler<SetReorderLevelCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public SetReorderLevelCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(SetReorderLevelCommand request, CancellationToken ct)
	{
		var item = await _context.InventoryItems
			.FirstOrDefaultAsync(i => i.Id == request.InventoryItemId && !i.IsDeleted, ct);
		if (item is null) return Error.NotFound("InventoryItem.NotFound", "موجودی یافت نشد.");

		item.ReorderLevel = request.ReorderLevel;
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
