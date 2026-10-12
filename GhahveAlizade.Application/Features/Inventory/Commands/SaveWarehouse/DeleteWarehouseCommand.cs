using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public record DeleteWarehouseCommand(long Id) : IRequest<ErrorOr<Success>>;
