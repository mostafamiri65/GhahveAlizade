using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

// Delete
public record DeleteCoffeeProcessCommand(long Id) : IRequest<ErrorOr<Success>>;
