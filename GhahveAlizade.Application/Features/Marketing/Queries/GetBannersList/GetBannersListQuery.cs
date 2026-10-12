using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannersList;

public record GetBannersListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	bool? IsActive = null,
	string? SortBy = "displayOrder",
	string? SortDir = "asc")
	: IRequest<ErrorOr<PaginatedList<BannerListItemDto>>>;