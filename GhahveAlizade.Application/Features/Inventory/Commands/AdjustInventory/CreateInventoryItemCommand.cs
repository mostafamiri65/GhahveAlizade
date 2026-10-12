using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public record CreateInventoryItemCommand(
	long ProductId,
	long? ProductVariantId,
	long WarehouseId,
	decimal InitialQuantity,
	decimal? ReorderLevel) : IRequest<ErrorOr<long>>;