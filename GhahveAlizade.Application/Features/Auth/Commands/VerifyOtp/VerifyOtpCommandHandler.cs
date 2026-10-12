using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ErrorOr<VerifyOtpResponse>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IOtpService _otpService;
	private readonly IDateTimeProvider _dateTime;

	public VerifyOtpCommandHandler(
		UserManager<ApplicationUser> userManager,
		IOtpService otpService,
		IDateTimeProvider dateTime)
	{
		_userManager = userManager;
		_otpService = otpService;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<VerifyOtpResponse>> Handle(
		VerifyOtpCommand request,
		CancellationToken cancellationToken)
	{
		var phone = NormalizePhone(request.PhoneNumber);

		// تایید OTP
		var verifyResult = await _otpService.VerifyAsync(
			phone, request.Code, purpose: "login", cancellationToken);

		if (!verifyResult.Success)
			return Error.Validation("Otp.Invalid", verifyResult.ErrorMessage ?? "کد وارد شده صحیح نیست.");

		// پیدا کردن یا ساخت کاربر
		var user = await _userManager.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == phone && !u.IsDeleted, cancellationToken);

		var isNewUser = false;

		if (user is null)
		{
			// ساخت کاربر جدید
			isNewUser = true;
			user = new ApplicationUser
			{
				UserName = phone,
				PhoneNumber = phone,
				PhoneNumberConfirmed = true,
				IsPhoneVerified = true,
				PhoneVerifiedAtUtc = _dateTime.UtcNow,
				HasPassword = false,
				IsActive = true,
				CreatedAtUtc = _dateTime.UtcNow,
				CreatedBy = 1
			};

			var createResult = await _userManager.CreateAsync(user);
			if (!createResult.Succeeded)
			{
				var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
				return Error.Failure("User.CreateFailed", errors);
			}

			await _userManager.AddToRoleAsync(user, "Customer");
		}
		else
		{
			// کاربر موجود
			if (!user.IsPhoneVerified)
			{
				user.IsPhoneVerified = true;
				user.PhoneVerifiedAtUtc = _dateTime.UtcNow;
				user.PhoneNumberConfirmed = true;
			}
		}

		// آپدیت آخرین ورود
		user.LastLoginAtUtc = _dateTime.UtcNow;
		user.LastLoginProvider = "OTP";
		await _userManager.UpdateAsync(user);

		return new VerifyOtpResponse(
			UserId: user.Id,
			PhoneNumber: phone,
			FullName: user.FullName,
			HasPassword: user.HasPassword,
			IsNewUser: isNewUser);
	}

	private static string NormalizePhone(string phone)
	{
		if (string.IsNullOrWhiteSpace(phone)) return string.Empty;
		phone = phone.Trim().Replace(" ", "").Replace("-", "");
		if (phone.StartsWith("+98")) phone = "0" + phone[3..];
		else if (phone.StartsWith("0098")) phone = "0" + phone[4..];
		else if (phone.StartsWith("98") && phone.Length == 12) phone = "0" + phone[2..];
		else if (phone.StartsWith("9") && phone.Length == 10) phone = "0" + phone;
		return phone;
	}
}