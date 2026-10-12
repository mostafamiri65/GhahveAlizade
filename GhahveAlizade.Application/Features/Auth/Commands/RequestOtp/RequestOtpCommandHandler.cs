using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Auth.Commands.RequestOtp;

public class RequestOtpCommandHandler : IRequestHandler<RequestOtpCommand, ErrorOr<RequestOtpResponse>>
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IOtpService _otpService;
	private readonly ISmsService _smsService;
	private readonly ICurrentUserService _currentUser;

	public RequestOtpCommandHandler(
		UserManager<ApplicationUser> userManager,
		IOtpService otpService,
		ISmsService smsService,
		ICurrentUserService currentUser)
	{
		_userManager = userManager;
		_otpService = otpService;
		_smsService = smsService;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<RequestOtpResponse>> Handle(
		RequestOtpCommand request,
		CancellationToken cancellationToken)
	{
		var phone = NormalizePhone(request.PhoneNumber);

		if (string.IsNullOrWhiteSpace(phone) || !IsValidIranianMobile(phone))
			return Error.Validation("Phone.Invalid", "شماره موبایل وارد شده معتبر نیست.");

		// اگه کاربر با این شماره وجود داره، بررسی فعال بودن
		var existingUser = await _userManager.Users
			.FirstOrDefaultAsync(u => u.PhoneNumber == phone && !u.IsDeleted, cancellationToken);

		if (existingUser is not null && !existingUser.IsActive)
			return Error.Forbidden("User.Inactive", "حساب کاربری شما غیرفعال است. با پشتیبانی تماس بگیرید.");

		// تولید OTP
		string code;
		try
		{
			code = await _otpService.GenerateAsync(
				phone,
				purpose: "login",
				ipAddress: _currentUser.IpAddress,
				userAgent: _currentUser.UserAgent,
				cancellationToken);
		}
		catch (InvalidOperationException ex)
		{
			return Error.Conflict("Otp.Cooldown", ex.Message);
		}

		// ارسال پیامک
		var smsResult = await _smsService.SendOtpAsync(phone, code, cancellationToken);

		if (!smsResult.Success)
			return Error.Failure("Sms.Failed", smsResult.ErrorMessage ?? "خطا در ارسال پیامک");

		return new RequestOtpResponse(
			PhoneNumber: phone,
			ExpiresInSeconds: 180,
			IsNewUser: existingUser is null);
	}

	private static string NormalizePhone(string phone)
	{
		if (string.IsNullOrWhiteSpace(phone)) return string.Empty;

		phone = phone.Trim()
			.Replace(" ", "")
			.Replace("-", "")
			.Replace("(", "")
			.Replace(")", "");

		// تبدیل به فرمت 09xxxxxxxxx
		if (phone.StartsWith("+98")) phone = "0" + phone[3..];
		else if (phone.StartsWith("0098")) phone = "0" + phone[4..];
		else if (phone.StartsWith("98") && phone.Length == 12) phone = "0" + phone[2..];
		else if (phone.StartsWith("9") && phone.Length == 10) phone = "0" + phone;

		return phone;
	}

	private static bool IsValidIranianMobile(string phone)
		=> System.Text.RegularExpressions.Regex.IsMatch(phone, @"^09[0-9]{9}$");
}