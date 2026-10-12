using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, ErrorOr<CurrentUserDto>>
{
	private readonly IIdentityService _identityService;
	private readonly IAppDbContext _context;

	public GetCurrentUserQueryHandler(
		IIdentityService identityService,
		IAppDbContext context)
	{
		_identityService = identityService;
		_context = context;
	}

	public async Task<ErrorOr<CurrentUserDto>> Handle(
		GetCurrentUserQuery request,
		CancellationToken cancellationToken)
	{
		var user = await _identityService.FindByIdAsync(request.UserId);
		if (user is null)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var roles = await _identityService.GetRolesAsync(request.UserId);

		var permissions = await _context.RolePermissions
			.Where(rp => roles.Contains(rp.Role.Name!) && !rp.IsDeleted)
			.Select(rp => rp.Permission.Name)
			.Distinct()
			.ToListAsync(cancellationToken);

		return new CurrentUserDto(
			Id: user.Id,
			UserName: user.UserName,
			Email: user.Email,
			PhoneNumber: user.PhoneNumber,
			FullName: user.FullName,
			AvatarUrl: user.AvatarUrl,
			HasPassword: user.HasPassword,
			IsPhoneVerified: user.IsPhoneVerified,
			Roles: roles.ToList(),
			Permissions: permissions);
	}
}