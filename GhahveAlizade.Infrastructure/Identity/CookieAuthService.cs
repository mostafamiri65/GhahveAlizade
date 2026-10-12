using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Infrastructure.Identity;

public class CookieAuthService : ICookieAuthService
{
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;

	public CookieAuthService(
		SignInManager<ApplicationUser> signInManager,
		UserManager<ApplicationUser> userManager)
	{
		_signInManager = signInManager;
		_userManager = userManager;
	}

	public async Task SignInAsync(
		long userId,
		bool isPersistent = false,
		CancellationToken cancellationToken = default)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return;

		await _signInManager.SignInAsync(user, isPersistent);
	}

	public Task SignOutAsync(CancellationToken cancellationToken = default)
		=> _signInManager.SignOutAsync();
}