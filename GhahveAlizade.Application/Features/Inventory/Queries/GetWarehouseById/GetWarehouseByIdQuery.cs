using ErrorOr;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetWarehouseById;

public record GetWarehouseByIdQuery(long Id) : IRequest<ErrorOr<WarehouseDto>>;
