using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Promotion.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Promotion.Queries.GetCouponsList;

public record GetCouponsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	bool? IsActive = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<CouponListItemDto>>>;