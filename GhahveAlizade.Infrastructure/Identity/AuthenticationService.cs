using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GhahveAlizade.Infrastructure.Identity;

public class AuthenticationService : IAuthenticationService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IJwtTokenService _jwtService;
	private readonly JwtSettings _jwtSettings;
	private readonly ICurrentUserService _currentUser;

	public AuthenticationService(
		UserManager<ApplicationUser> userManager,
		IJwtTokenService jwtService,
		IOptions<JwtSettings> jwtSettings,
		ICurrentUserService currentUser)
	{
		_userManager = userManager;
		_jwtService = jwtService;
		_jwtSettings = jwtSettings.Value;
		_currentUser = currentUser;
	}

	public async Task<ErrorOr<long>> ValidatePasswordAsync(
		string usernameOrEmail,
		string password,
		CancellationToken cancellationToken = default)
	{
		var input = usernameOrEmail.Trim();

		var user = input.Contains('@')
			? await _userManager.FindByEmailAsync(input)
			: await _userManager.FindByNameAsync(input);

		if (user is null || user.IsDeleted)
			return Error.Unauthorized("Auth.InvalidCredentials", "نام کاربری یا رمز عبور اشتباه است.");

		if (!user.IsActive)
			return Error.Forbidden("User.Inactive", "حساب کاربری شما غیرفعال است.");

		if (!user.HasPassword || string.IsNullOrEmpty(user.PasswordHash))
			return Error.Validation("Auth.NoPassword", "برای این حساب رمز عبوری تعیین نشده. با پیامک وارد شوید.");

		var passwordValid = await _userManager.CheckPasswordAsync(user, password);
		if (!passwordValid)
			return Error.Unauthorized("Auth.InvalidCredentials", "نام کاربری یا رمز عبور اشتباه است.");

		// آپدیت آخرین ورود
		user.LastLoginAtUtc = DateTime.UtcNow;
		user.LastLoginProvider = "Password";
		await _userManager.UpdateAsync(user);

		return user.Id;
	}

	public async Task<ErrorOr<AuthTokens>> GenerateTokensAsync(
		long userId,
		CancellationToken cancellationToken = default)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user is null || user.IsDeleted)
			return Error.NotFound("User.NotFound", "کاربر یافت نشد.");

		var (accessToken, refreshToken) = await _jwtService.GenerateTokensAsync(user);

		return new AuthTokens(
			AccessToken: accessToken,
			RefreshToken: refreshToken,
			ExpiresInSeconds: _jwtSettings.AccessTokenExpirationMinutes * 60,
			UserId: user.Id);
	}

	public async Task<ErrorOr<AuthTokens>> RefreshTokensAsync(
		string refreshToken,
		CancellationToken cancellationToken = default)
	{
		var user = await _jwtService.ValidateRefreshTokenAsync(refreshToken);
		if (user is null)
			return Error.Unauthorized("Auth.InvalidRefreshToken", "Refresh Token معتبر نیست.");

		// ابطال توکن قدیمی
		await _jwtService.RevokeRefreshTokenAsync(refreshToken);

		// ساخت جفت توکن جدید
		var (accessToken, newRefreshToken) = await _jwtService.GenerateTokensAsync(user);

		return new AuthTokens(
			AccessToken: accessToken,
			RefreshToken: newRefreshToken,
			ExpiresInSeconds: _jwtSettings.AccessTokenExpirationMinutes * 60,
			UserId: user.Id);
	}

	public async Task<ErrorOr<Success>> RevokeRefreshTokenAsync(
		string refreshToken,
		CancellationToken cancellationToken = default)
	{
		await _jwtService.RevokeRefreshTokenAsync(refreshToken);
		return Result.Success;
	}

	public async Task<IdentityUserDto?> GetCurrentUserAsync(long? userId = null)
	{
		var id = userId ?? _currentUser.UserId;
		if (id is null) return null;

		var user = await _userManager.FindByIdAsync(id.Value.ToString());
		if (user is null || user.IsDeleted) return null;

		return new IdentityUserDto(
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

}