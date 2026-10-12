using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Wholesale.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestsList;

public record GetWholesaleRequestsListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	WholesaleRequestStatus? Status = null,
	string? SortBy = "createdAt", string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<WholesaleRequestListItemDto>>>;
