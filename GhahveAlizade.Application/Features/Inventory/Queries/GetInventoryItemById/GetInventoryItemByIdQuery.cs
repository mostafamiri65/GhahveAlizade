using ErrorOr;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetInventoryItemById;

public record GetInventoryItemByIdQuery(long Id) : IRequest<ErrorOr<InventoryItemDto>>;
