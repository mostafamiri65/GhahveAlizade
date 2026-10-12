using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public record ToggleWarehouseActiveCommand(long Id) : IRequest<ErrorOr<bool>>;