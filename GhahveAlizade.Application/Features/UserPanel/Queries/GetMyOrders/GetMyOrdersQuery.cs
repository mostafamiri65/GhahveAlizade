using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrders;

public record GetMyOrdersQuery(
	long UserId,
	int PageNumber = 1,
	int PageSize = 10,
	OrderStatus? Status = null)
	: IRequest<ErrorOr<PaginatedList<MyOrderListItemDto>>>;
