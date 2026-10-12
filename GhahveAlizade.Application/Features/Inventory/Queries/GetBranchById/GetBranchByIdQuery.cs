using ErrorOr;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetBranchById;

public record GetBranchByIdQuery(long Id) : IRequest<ErrorOr<BranchDto>>;
