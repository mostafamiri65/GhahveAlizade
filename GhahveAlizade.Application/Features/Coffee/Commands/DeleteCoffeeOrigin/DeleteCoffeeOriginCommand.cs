using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Commands.DeleteCoffeeOrigin;

public record DeleteCoffeeOriginCommand(long Id) : IRequest<ErrorOr<Success>>;