using ErrorOr;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeProcesses;

public record GetCoffeeProcessByIdQuery(long Id) : IRequest<ErrorOr<CoffeeProcessDto>>;
