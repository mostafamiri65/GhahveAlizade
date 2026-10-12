using ErrorOr;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Identity.Users.Commands.AdminResetPassword;

public class AdminResetPasswordCommandHandler
	: IRequestHandler<AdminResetPasswordCommand, ErrorOr<Success>>
{
	private readonly UserManager<ApplicationUser> _userManager;

	public AdminResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
		=> _userManager = userManager;

	public async Task<ErrorOr<Success>> Handle(
		AdminResetPasswordCommand request, CancellationToken ct)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		// اگه پسورد داره، حذف و دوباره
		if (user.HasPassword)
			await _userManager.RemovePasswordAsync(user);

		var result = await _userManager.AddPasswordAsync(user, request.NewPassword);
		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();
			return errors;
		}

		user.HasPassword = true;
		await _userManager.UpdateAsync(user);
		return Result.Success;
	}
}