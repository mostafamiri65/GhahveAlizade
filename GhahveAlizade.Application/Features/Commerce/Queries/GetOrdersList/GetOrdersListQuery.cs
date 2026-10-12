using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Commerce.Queries.GetOrdersList;

public record GetOrdersListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	OrderStatus? Status = null,
	DateTime? FromDate = null,
	DateTime? ToDate = null,
	decimal? MinTotal = null,
	decimal? MaxTotal = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<OrderListItemDto>>>;