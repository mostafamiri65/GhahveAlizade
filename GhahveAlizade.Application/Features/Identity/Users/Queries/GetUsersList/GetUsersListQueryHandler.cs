using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Identity.Users.Dtos;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Identity.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler
	: IRequestHandler<GetUsersListQuery, ErrorOr<PaginatedList<UserListItemDto>>>
{
	private readonly UserManager<ApplicationUser> _userManager;

	public GetUsersListQueryHandler(UserManager<ApplicationUser> userManager)
		=> _userManager = userManager;

	public async Task<ErrorOr<PaginatedList<UserListItemDto>>> Handle(
		GetUsersListQuery request,
		CancellationToken cancellationToken)
	{
		var query = _userManager.Users
			.AsNoTracking()
			.Where(u => !u.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(u =>
				(u.UserName != null && u.UserName.Contains(s)) ||
				(u.Email != null && u.Email.Contains(s)) ||
				(u.PhoneNumber != null && u.PhoneNumber.Contains(s)) ||
				(u.FirstName != null && u.FirstName.Contains(s)) ||
				(u.LastName != null && u.LastName.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(u => u.IsActive == request.IsActive.Value);

		// فیلتر بر اساس Role
		if (!string.IsNullOrWhiteSpace(request.Role))
		{
			var userIdsInRole = await _userManager.GetUsersInRoleAsync(request.Role);
			var ids = userIdsInRole.Select(u => u.Id).ToList();
			query = query.Where(u => ids.Contains(u.Id));
		}

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("username", "asc") => query.OrderBy(u => u.UserName),
			("username", _) => query.OrderByDescending(u => u.UserName),
			("email", "asc") => query.OrderBy(u => u.Email),
			("email", _) => query.OrderByDescending(u => u.Email),
			("lastlogin", "asc") => query.OrderBy(u => u.LastLoginAtUtc),
			("lastlogin", _) => query.OrderByDescending(u => u.LastLoginAtUtc),
			("createdat", "asc") => query.OrderBy(u => u.CreatedAtUtc),
			_ => query.OrderByDescending(u => u.CreatedAtUtc)
		};

		var total = await query.CountAsync(cancellationToken);
		var users = await query
			.Skip((request.PageNumber - 1) * request.PageSize)
			.Take(request.PageSize)
			.ToListAsync(cancellationToken);

		// گرفتن نقش‌ها
		var result = new List<UserListItemDto>();
		foreach (var u in users)
		{
			var roles = await _userManager.GetRolesAsync(u);
			result.Add(new UserListItemDto
			{
				Id = u.Id,
				UserName = u.UserName ?? "",
				Email = u.Email,
				PhoneNumber = u.PhoneNumber,
				FirstName = u.FirstName,
				LastName = u.LastName,
				FullName = u.FullName,
				IsActive = u.IsActive,
				IsPhoneVerified = u.IsPhoneVerified,
				HasPassword = u.HasPassword,
				LastLoginAtUtc = u.LastLoginAtUtc,
				LastLoginProvider = u.LastLoginProvider,
				CreatedAtUtc = u.CreatedAtUtc,
				Roles = roles.ToList()
			});
		}

		return new PaginatedList<UserListItemDto>(result, total, request.PageNumber, request.PageSize);
	}
}