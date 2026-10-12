using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public record SetReorderLevelCommand(
	long InventoryItemId,
	decimal? ReorderLevel) : IRequest<ErrorOr<Success>>;
