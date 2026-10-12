using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;
public record ToggleBranchActiveCommand(long Id) : IRequest<ErrorOr<bool>>;