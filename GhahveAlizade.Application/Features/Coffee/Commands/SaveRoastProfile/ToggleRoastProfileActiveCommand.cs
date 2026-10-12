using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;
public record ToggleRoastProfileActiveCommand(long Id) : IRequest<ErrorOr<bool>>;