using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.ToggleUserActive;

public class ToggleUserActiveCommandHandler
	: IRequestHandler<ToggleUserActiveCommand, ErrorOr<bool>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ICurrentUserService _currentUser;

	public ToggleUserActiveCommandHandler(
		UserManager<ApplicationUser> userManager,
		ICurrentUserService currentUser)
	{
		_userManager = userManager;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<bool>> Handle(ToggleUserActiveCommand request, CancellationToken ct)
	{
		// نمی‌تونی خودت رو غیرفعال کنی
		if (_currentUser.UserId == request.Id)
			return Error.Conflict("User.CannotSelfDeactivate", "نمی‌توانید حساب خودتان را غیرفعال کنید.");

		var user = await _userManager.FindByIdAsync(request.Id.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		user.IsActive = !user.IsActive;
		await _userManager.UpdateAsync(user);
		return user.IsActive;
	}
}