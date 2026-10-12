using ErrorOr;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Commerce.Queries.GetOrderById;

public record GetOrderByIdQuery(long Id) : IRequest<ErrorOr<OrderDetailDto>>;
