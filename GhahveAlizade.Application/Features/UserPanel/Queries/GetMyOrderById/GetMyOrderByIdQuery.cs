using ErrorOr;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrderById;

public record GetMyOrderByIdQuery(long UserId, long OrderId)
	: IRequest<ErrorOr<OrderDetailDto>>;
