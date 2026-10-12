using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Identity.Users.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Identity.Users.Queries.GetUsersList;

public record GetUsersListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	string? Role = null,
	bool? IsActive = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<UserListItemDto>>>;