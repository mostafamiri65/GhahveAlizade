using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Reviews.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Reviews.Queries.GetReviewsList;

public record GetReviewsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? ProductId = null,
	int? Rating = null,
	bool? IsApproved = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<ReviewListItemDto>>>;