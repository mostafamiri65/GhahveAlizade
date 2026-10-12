using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
	private readonly UserManager<ApplicationUser> _userManager;

	public IdentityService(UserManager<ApplicationUser> userManager)
		=> _userManager = userManager;

	public async Task<IdentityUserDto?> FindByIdAsync(long userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		return user is null || user.IsDeleted ? null : MapToDto(user);
	}

	public async Task<IdentityUserDto?> FindByPhoneAsync(string phoneNumber)
	{
		var user = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber && !u.IsDeleted);
		return user is null ? null : MapToDto(user);
	}

	public async Task<IdentityUserDto?> FindByEmailAsync(string email)
	{
		var user = await _userManager.FindByEmailAsync(email);
		return user is null || user.IsDeleted ? null : MapToDto(user);
	}

	public async Task<IdentityUserDto?> FindByUserNameAsync(string userName)
	{
		var user = await _userManager.FindByNameAsync(userName);
		return user is null || user.IsDeleted ? null : MapToDto(user);
	}

	public async Task<IReadOnlyList<string>> GetRolesAsync(long userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Array.Empty<string>();
		return (await _userManager.GetRolesAsync(user)).ToList();
	}

	public async Task<bool> IsInRoleAsync(long userId, string role)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		return user is not null && await _userManager.IsInRoleAsync(user, role);
	}

	public async Task<ErrorOr<long>> CreateUserAsync(
		string userName,
		string? phoneNumber,
		string? email,
		string? password = null)
	{
		var user = new ApplicationUser
		{
			UserName = userName,
			PhoneNumber = phoneNumber,
			Email = email,
			PhoneNumberConfirmed = phoneNumber is not null,
			IsPhoneVerified = phoneNumber is not null,
			PhoneVerifiedAtUtc = phoneNumber is not null ? DateTime.UtcNow : null,
			HasPassword = password is not null,
			IsActive = true,
			CreatedAtUtc = DateTime.UtcNow,
			CreatedBy = 1
		};

		var result = password is not null
			? await _userManager.CreateAsync(user, password)
			: await _userManager.CreateAsync(user);

		if (!result.Succeeded)
		{
			var errors = result.Errors
				.Select(e => Error.Validation(e.Code, e.Description))
				.ToList();
			return errors;
		}

		return user.Id;
	}

	public async Task<ErrorOr<Success>> AddToRoleAsync(long userId, string role)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var result = await _userManager.AddToRoleAsync(user, role);
		if (!result.Succeeded)
		{
			var errors = result.Errors
				.Select(e => Error.Failure(e.Code, e.Description))
				.ToList();
			return errors;
		}

		return Result.Success;
	}

	public async Task<ErrorOr<Success>> UpdateUserAsync(long userId, Action<IdentityUserDto> update)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var dto = MapToDto(user);
		update(dto);

		user.FirstName = dto.FirstName;
		user.LastName = dto.LastName;
		user.Email = dto.Email;
		user.PhoneNumber = dto.PhoneNumber;
		user.AvatarUrl = dto.AvatarUrl;
		user.IsActive = dto.IsActive;
		user.UpdatedAtUtc = DateTime.UtcNow;

		var result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			var errors = result.Errors
				.Select(e => Error.Failure(e.Code, e.Description))
				.ToList();
			return errors;
		}

		return Result.Success;
	}

	public async Task<ErrorOr<Success>> SetPasswordAsync(long userId, string password)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		if (user.HasPassword)
		{
			var removeResult = await _userManager.RemovePasswordAsync(user);
			if (!removeResult.Succeeded)
				return Error.Failure("Password.RemoveFailed", "خطا در تغییر رمز عبور.");
		}

		var result = await _userManager.AddPasswordAsync(user, password);
		if (!result.Succeeded)
		{
			var errors = result.Errors
				.Select(e => Error.Validation(e.Code, e.Description))
				.ToList();
			return errors;
		}

		user.HasPassword = true;
		user.UpdatedAtUtc = DateTime.UtcNow;
		await _userManager.UpdateAsync(user);

		return Result.Success;
	}

	public async Task<ErrorOr<Success>> RemovePasswordAsync(long userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var result = await _userManager.RemovePasswordAsync(user);
		if (!result.Succeeded)
			return Error.Failure("Password.RemoveFailed", "خطا در حذف رمز عبور.");

		user.HasPassword = false;
		user.UpdatedAtUtc = DateTime.UtcNow;
		await _userManager.UpdateAsync(user);

		return Result.Success;
	}

	public async Task<bool> CheckPasswordAsync(long userId, string password)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		return user is not null && await _userManager.CheckPasswordAsync(user, password);
	}

	public async Task<ErrorOr<Success>> SetPhoneVerifiedAsync(long userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		user.IsPhoneVerified = true;
		user.PhoneVerifiedAtUtc = DateTime.UtcNow;
		user.PhoneNumberConfirmed = true;
		user.UpdatedAtUtc = DateTime.UtcNow;

		await _userManager.UpdateAsync(user);
		return Result.Success;
	}

	public async Task<ErrorOr<Success>> RecordLoginAsync(long userId, string provider)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null) return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		user.LastLoginAtUtc = DateTime.UtcNow;
		user.LastLoginProvider = provider;
		await _userManager.UpdateAsync(user);

		return Result.Success;
	}

	private static IdentityUserDto MapToDto(ApplicationUser user)
		=> new(
			Id: user.Id,
			UserName: user.UserName ?? string.Empty,
			Email: user.Email,
			PhoneNumber: user.PhoneNumber,
			FirstName: user.FirstName,
			LastName: user.LastName,
			FullName: user.FullName,
			AvatarUrl: user.AvatarUrl,
			HasPassword: user.HasPassword,
			IsPhoneVerified: user.IsPhoneVerified,
			IsActive: user.IsActive,
			IsDeleted: user.IsDeleted,
			LastLoginAtUtc: user.LastLoginAtUtc,
			LastLoginProvider: user.LastLoginProvider);
}