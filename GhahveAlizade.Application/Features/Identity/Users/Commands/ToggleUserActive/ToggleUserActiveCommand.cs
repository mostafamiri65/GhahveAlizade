using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.ToggleUserActive;

public record ToggleUserActiveCommand(long Id) : IRequest<ErrorOr<bool>>;