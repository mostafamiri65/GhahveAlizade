using ErrorOr;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeOriginById;

public record GetCoffeeOriginByIdQuery(long Id) : IRequest<ErrorOr<CoffeeOriginDto>>;