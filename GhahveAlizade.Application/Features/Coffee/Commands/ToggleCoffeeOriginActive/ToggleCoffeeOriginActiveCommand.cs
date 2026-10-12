using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.ToggleCoffeeOriginActive;

public record ToggleCoffeeOriginActiveCommand(long Id) : IRequest<ErrorOr<bool>>;