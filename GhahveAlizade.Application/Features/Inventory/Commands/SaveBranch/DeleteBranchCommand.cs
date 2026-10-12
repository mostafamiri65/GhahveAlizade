using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public record DeleteBranchCommand(long Id) : IRequest<ErrorOr<Success>>;
