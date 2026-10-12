using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Commands.LoginWithPassword;

public class LoginWithPasswordCommandHandler : IRequestHandler<LoginWithPasswordCommand, ErrorOr<LoginWithPasswordResponse>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IDateTimeProvider _dateTime;

	public LoginWithPasswordCommandHandler(
		UserManager<ApplicationUser> userManager,
		IDateTimeProvider dateTime)
	{
		_userManager = userManager;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<LoginWithPasswordResponse>> Handle(
		LoginWithPasswordCommand request,
		CancellationToken cancellationToken)
	{
		var input = request.UsernameOrEmail.Trim();

		ApplicationUser? user = input.Contains('@')
			? await _userManager.FindByEmailAsync(input)
			: await _userManager.FindByNameAsync(input);

		if (user is null || user.IsDeleted)
			return Error.Unauthorized("Auth.InvalidCredentials", "نام کاربری یا رمز عبور اشتباه است.");

		if (!user.IsActive)
			return Error.Forbidden("User.Inactive", "حساب کاربری شما غیرفعال است.");

		if (!user.HasPassword || string.IsNullOrEmpty(user.PasswordHash))
			return Error.Validation(
				"Auth.NoPassword",
				"برای این حساب پسوردی تعیین نشده. لطفاً با پیامک وارد شوید.");

		var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
		if (!passwordValid)
			return Error.Unauthorized("Auth.InvalidCredentials", "نام کاربری یا رمز عبور اشتباه است.");

		// آپدیت آخرین ورود
		user.LastLoginAtUtc = _dateTime.UtcNow;
		user.LastLoginProvider = "Password";
		await _userManager.UpdateAsync(user);

		return new LoginWithPasswordResponse(
			UserId: user.Id,
			UserName: user.UserName!,
			FullName: user.FullName,
			Email: user.Email);
	}
}