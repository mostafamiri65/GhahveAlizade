using ErrorOr;

namespace GhahveAlizade.Application.Common.Interfaces;

/// <summary>
/// سرویس احراز هویت — Application فقط این رو می‌بینه، نه JWT نه Cookie.
/// </summary>
public interface IAuthenticationService
{
	/// <summary>
	/// بررسی رمز عبور و برگرداندن UserId در صورت موفقیت.
	/// </summary>
	Task<ErrorOr<long>> ValidatePasswordAsync(
		string usernameOrEmail,
		string password,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// ساخت توکن‌های دسترسی برای کاربر.
	/// </summary>
	Task<ErrorOr<AuthTokens>> GenerateTokensAsync(
		long userId,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// اعتبارسنجی Refresh Token و ساخت توکن جدید.
	/// </summary>
	Task<ErrorOr<AuthTokens>> RefreshTokensAsync(
		string refreshToken,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// ابطال Refresh Token (خروج).
	/// </summary>
	Task<ErrorOr<Success>> RevokeRefreshTokenAsync(
		string refreshToken,
		CancellationToken cancellationToken = default);
	Task<IdentityUserDto?> GetCurrentUserAsync(long? userId = null);
}

public record AuthTokens(
	string AccessToken,
	string RefreshToken,
	int ExpiresInSeconds,
	long UserId);