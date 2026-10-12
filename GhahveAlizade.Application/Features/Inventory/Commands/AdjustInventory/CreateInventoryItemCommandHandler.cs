using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Inventory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public class CreateInventoryItemCommandHandler
	: IRequestHandler<CreateInventoryItemCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	private readonly ICurrentUserService _currentUser;

	public CreateInventoryItemCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
	{
		_context = context;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<long>> Handle(CreateInventoryItemCommand request, CancellationToken ct)
	{
		// چک وجود نداشتن قبلی
		var exists = await _context.InventoryItems.AnyAsync(i =>
			i.ProductId == request.ProductId &&
			i.ProductVariantId == request.ProductVariantId &&
			i.WarehouseId == request.WarehouseId &&
			!i.IsDeleted, ct);

		if (exists)
			return Error.Conflict("InventoryItem.Exists", "این آیتم در این انبار قبلاً تعریف شده است.");

		var item = new InventoryItem
		{
			ProductId = request.ProductId,
			ProductVariantId = request.ProductVariantId,
			WarehouseId = request.WarehouseId,
			Quantity = request.InitialQuantity,
			ReservedQuantity = 0,
			ReorderLevel = request.ReorderLevel
		};

		_context.InventoryItems.Add(item);
		await _context.SaveChangesAsync(ct);

		if (request.InitialQuantity > 0)
		{
			_context.InventoryTransactions.Add(new InventoryTransaction
			{
				InventoryItemId = item.Id,
				TransactionType = "Initial",
				Quantity = request.InitialQuantity,
				BalanceAfter = request.InitialQuantity,
				ReferenceType = "Manual",
				Notes = "موجودی اولیه",
				CreatedAtUtc = DateTime.UtcNow,
				CreatedBy = _currentUser.UserId
			});
			await _context.SaveChangesAsync(ct);
		}

		return item.Id;
	}
}