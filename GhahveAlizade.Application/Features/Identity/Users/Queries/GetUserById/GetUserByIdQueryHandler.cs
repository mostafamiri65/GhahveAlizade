using ErrorOr;
using GhahveAlizade.Application.Features.Identity.Users.Dtos;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
	: IRequestHandler<GetUserByIdQuery, ErrorOr<UserDetailDto>>
{
	private readonly UserManager<ApplicationUser> _userManager;

	public GetUserByIdQueryHandler(UserManager<ApplicationUser> userManager)
		=> _userManager = userManager;

	public async Task<ErrorOr<UserDetailDto>> Handle(
		GetUserByIdQuery request, CancellationToken ct)
	{
		var u = await _userManager.FindByIdAsync(request.Id.ToString());
		if (u is null || u.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var roles = await _userManager.GetRolesAsync(u);

		return new UserDetailDto
		{
			Id = u.Id,
			UserName = u.UserName ?? "",
			Email = u.Email,
			PhoneNumber = u.PhoneNumber,
			FirstName = u.FirstName,
			LastName = u.LastName,
			FullName = u.FullName,
			AvatarUrl = u.AvatarUrl,
			NationalCode = u.NationalCode,
			BirthDate = u.BirthDate,
			Gender = u.Gender,
			IsActive = u.IsActive,
			IsPhoneVerified = u.IsPhoneVerified,
			HasPassword = u.HasPassword,
			LastLoginAtUtc = u.LastLoginAtUtc,
			LastLoginProvider = u.LastLoginProvider,
			CreatedAtUtc = u.CreatedAtUtc,
			Roles = roles.ToList()
		};
	}
}