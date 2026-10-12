using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

public record ToggleCoffeeProcessActiveCommand(long Id) : IRequest<ErrorOr<bool>>;