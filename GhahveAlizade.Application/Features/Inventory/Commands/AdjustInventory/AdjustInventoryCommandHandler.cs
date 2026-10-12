using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Inventory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public class AdjustInventoryCommandHandler
	: IRequestHandler<AdjustInventoryCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public AdjustInventoryCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<Success>> Handle(AdjustInventoryCommand request, CancellationToken ct)
	{
		var item = await _context.InventoryItems
			.FirstOrDefaultAsync(i => i.Id == request.InventoryItemId && !i.IsDeleted, ct);

		if (item is null) return Error.NotFound("InventoryItem.NotFound", "موجودی یافت نشد.");

		var previousQuantity = item.Quantity;
		var diff = request.NewQuantity - previousQuantity;

		if (diff == 0) return Result.Success;

		item.Quantity = request.NewQuantity;

		// ثبت تراکنش
		_context.InventoryTransactions.Add(new InventoryTransaction
		{
			InventoryItemId = item.Id,
			TransactionType = diff > 0 ? "Adjust-In" : "Adjust-Out",
			Quantity = diff,
			BalanceAfter = request.NewQuantity,
			ReferenceType = "Manual",
			Notes = request.Notes ?? "تعدیل دستی",
			CreatedAtUtc = DateTime.UtcNow,
			CreatedBy = _currentUser.UserId
		});

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
