using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Auth.Commands.CompleteProfile;
using GhahveAlizade.Application.Features.Auth.Commands.LoginWithPassword;
using GhahveAlizade.Application.Features.Auth.Commands.RequestOtp;
using GhahveAlizade.Application.Features.Auth.Commands.SetPassword;
using GhahveAlizade.Application.Features.Auth.Commands.VerifyOtp;
using GhahveAlizade.Web.Areas.Identity.Models;
using GhahveAlizade.Web.Controllers;
using GhahveAlizade.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GhahveAlizade.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Route("account")]
public class AccountController : MvcBaseController
{
	private readonly IAuthenticationService _authService;
	private readonly ICookieAuthService _cookieAuth;
	private readonly ICurrentUserService _currentUser;

	public AccountController(
		IAuthenticationService authService,
		ICookieAuthService cookieAuth,
		ICurrentUserService currentUser)
	{
		_authService = authService;
		_cookieAuth = cookieAuth;
		_currentUser = currentUser;
	}

	// ==================== ورود ====================

	[HttpGet("login")]
	[AllowAnonymous]
	public IActionResult Login(string? returnUrl = null)
	{
		if (_currentUser.IsAuthenticated)
			return RedirectToLocal(returnUrl);

		return View(new LoginViewModel { ReturnUrl = returnUrl });
	}

	[HttpPost("login")]
	[AllowAnonymous]
	[EnableRateLimiting(RateLimitingExtensions.OtpPolicy)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginViewModel model, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		var result = await Mediator.Send(
			new RequestOtpCommand(model.PhoneNumber),
			cancellationToken);

		if (result.IsError)
		{
			ModelState.AddModelError(string.Empty, result.FirstError.Description);
			return View(model);
		}

		TempData["PhoneNumber"] = result.Value.PhoneNumber;
		TempData["IsNewUser"] = result.Value.IsNewUser;
		TempData["ReturnUrl"] = model.ReturnUrl;

		return RedirectToAction(nameof(VerifyOtp));
	}

	// ==================== تایید OTP ====================

	[HttpGet("verify-otp")]
	[AllowAnonymous]
	public IActionResult VerifyOtp()
	{
		var phone = TempData["PhoneNumber"]?.ToString();
		if (string.IsNullOrEmpty(phone))
			return RedirectToAction(nameof(Login));

		TempData.Keep("PhoneNumber");
		TempData.Keep("IsNewUser");
		TempData.Keep("ReturnUrl");

		return View(new VerifyOtpViewModel
		{
			PhoneNumber = phone,
			ReturnUrl = TempData["ReturnUrl"]?.ToString()
		});
	}

	[HttpPost("verify-otp")]
	[AllowAnonymous]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		var result = await Mediator.Send(
			new VerifyOtpCommand(model.PhoneNumber, model.Code),
			cancellationToken);

		if (result.IsError)
		{
			ModelState.AddModelError(string.Empty, result.FirstError.Description);
			TempData.Keep("PhoneNumber");
			return View(model);
		}

		// ورود با Cookie
		await _cookieAuth.SignInAsync(result.Value.UserId, isPersistent: true, cancellationToken);

		// کاربر جدید → تکمیل مشخصات
		if (result.Value.IsNewUser)
		{
			TempData["ReturnUrl"] = model.ReturnUrl;
			return RedirectToAction(nameof(CompleteProfile));
		}

		// کاربر قدیمی بدون پسورد → پیشنهاد تعیین پسورد
		if (!result.Value.HasPassword)
		{
			TempData["SetPasswordPrompt"] = true;
			TempData["ReturnUrl"] = model.ReturnUrl;
			return RedirectToAction(nameof(SetPassword));
		}

		return RedirectToLocal(model.ReturnUrl);
	}

	// ==================== تکمیل مشخصات (کاربر جدید) ====================

	[HttpGet("complete-profile")]
	[Authorize]
	public IActionResult CompleteProfile()
	{
		return View(new CompleteProfileViewModel
		{
			ReturnUrl = TempData["ReturnUrl"]?.ToString()
		});
	}

	[HttpPost("complete-profile")]
	[Authorize]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> CompleteProfile(
		CompleteProfileViewModel model, CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		var userId = _currentUser.UserId;
		if (userId is null)
			return Challenge();

		var result = await Mediator.Send(new CompleteProfileCommand(
			userId.Value, model.FirstName, model.LastName, model.Email,
			model.NationalCode, model.BirthDate, model.Gender), cancellationToken);

		if (result.IsError)
		{
			ModelState.AddModelError(string.Empty, result.FirstError.Description);
			return View(model);
		}

		TempData["SuccessMessage"] = "خوش آمدید! ثبت‌نام شما کامل شد.";

		// اگر پسورد ندارد، پیشنهاد تعیین پسورد
		var user = await _authService.GetCurrentUserAsync();
		if (user is not null && !user.HasPassword)
		{
			TempData["SetPasswordPrompt"] = true;
			TempData["ReturnUrl"] = model.ReturnUrl;
			return RedirectToAction(nameof(SetPassword));
		}

		return RedirectToLocal(model.ReturnUrl);
	}

	// ==================== ورود با رمز ====================

	[HttpGet("login-with-password")]
	[AllowAnonymous]
	public IActionResult LoginWithPassword(string? returnUrl = null)
	{
		if (_currentUser.IsAuthenticated)
			return RedirectToLocal(returnUrl);

		return View(new LoginWithPasswordViewModel { ReturnUrl = returnUrl });
	}

	[HttpPost("login-with-password")]
	[AllowAnonymous]
	[EnableRateLimiting(RateLimitingExtensions.LoginPolicy)]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> LoginWithPassword(
		LoginWithPasswordViewModel model,
		CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		var result = await Mediator.Send(
			new LoginWithPasswordCommand(model.UsernameOrEmail, model.Password),
			cancellationToken);

		if (result.IsError)
		{
			ModelState.AddModelError(string.Empty, result.FirstError.Description);
			return View(model);
		}

		await _cookieAuth.SignInAsync(result.Value.UserId, model.RememberMe, cancellationToken);

		return RedirectToLocal(model.ReturnUrl);
	}

	// ==================== تعیین/تغییر رمز ====================

	[HttpGet("set-password")]
	[Authorize]
	public IActionResult SetPassword()
	{
		// اگه از Redirect نیومده و کاربر قبلاً پسورد داره، اجازه بده
		return View(new SetPasswordViewModel());
	}

	[HttpPost("set-password")]
	[Authorize]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> SetPassword(
		SetPasswordViewModel model,
		CancellationToken cancellationToken)
	{
		if (!ModelState.IsValid)
			return View(model);

		var userId = _currentUser.UserId;
		if (userId is null)
			return Challenge();

		var result = await Mediator.Send(
			new SetPasswordCommand(userId.Value, model.NewPassword),
			cancellationToken);

		if (result.IsError)
		{
			ModelState.AddModelError(string.Empty, result.FirstError.Description);
			return View(model);
		}

		TempData["SuccessMessage"] = "رمز عبور شما با موفقیت ثبت شد.";
		TempData["ReturnUrl"] = TempData["ReturnUrl"];

		return RedirectToLocal(TempData["ReturnUrl"]?.ToString());
	}

	// ==================== خروج ====================

	[HttpPost("logout")]
	[Authorize]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout(CancellationToken cancellationToken)
	{
		await _cookieAuth.SignOutAsync(cancellationToken);
		return RedirectToAction("Index", "Home", new { area = "" });
	}

	// ==================== Access Denied ====================

	[HttpGet("access-denied")]
	[AllowAnonymous]
	public IActionResult AccessDenied() => View();

	// ==================== Helper ====================

	//private IActionResult RedirectToLocal(string? returnUrl)
	//{
	//	if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
	//		return Redirect(returnUrl);

	//	return RedirectToAction("Index", "Home", new { area = "" });
	//}
}
