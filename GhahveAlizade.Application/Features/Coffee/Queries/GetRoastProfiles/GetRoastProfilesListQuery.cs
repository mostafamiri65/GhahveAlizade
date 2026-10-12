using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetRoastProfiles;

public record GetRoastProfilesListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	bool? IsActive = null, string? SortBy = "displayOrder", string? SortDir = "asc")
	: IRequest<ErrorOr<PaginatedList<RoastProfileListItemDto>>>;
