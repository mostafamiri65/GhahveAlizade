using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.AdjustInventory;

public record AdjustInventoryCommand(
	long InventoryItemId,
	decimal NewQuantity,
	string? Notes) : IRequest<ErrorOr<Success>>;
