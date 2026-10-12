using ErrorOr;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerOrders;

public record GetCustomerOrdersQuery(long CustomerId, int Take = 20)
	: IRequest<ErrorOr<List<OrderListItemDto>>>;
