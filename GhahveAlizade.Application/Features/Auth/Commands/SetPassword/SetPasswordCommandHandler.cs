using ErrorOr;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Application.Features.Auth.Commands.SetPassword;

public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, ErrorOr<Success>>
{
	private readonly UserManager<ApplicationUser> _userManager;

	public SetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
		=> _userManager = userManager;

	public async Task<ErrorOr<Success>> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		// اگه پسورد داره، باید حذف بشه بعد جدید اضافه بشه
		if (user.HasPassword)
		{
			var removeResult = await _userManager.RemovePasswordAsync(user);
			if (!removeResult.Succeeded)
				return Error.Failure("Password.RemoveFailed", "خطا در تغییر پسورد");
		}

		var addResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
		if (!addResult.Succeeded)
		{
			var errors = string.Join(", ", addResult.Errors.Select(e => e.Description));
			return Error.Validation("Password.Weak", errors);
		}

		user.HasPassword = true;
		await _userManager.UpdateAsync(user);

		return Result.Success;
	}
}