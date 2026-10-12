using ErrorOr;

namespace GhahveAlizade.Application.Common.Interfaces;

public interface IIdentityService
{
	Task<IdentityUserDto?> FindByIdAsync(long userId);
	Task<IdentityUserDto?> FindByPhoneAsync(string phoneNumber);
	Task<IdentityUserDto?> FindByEmailAsync(string email);
	Task<IdentityUserDto?> FindByUserNameAsync(string userName);

	Task<IReadOnlyList<string>> GetRolesAsync(long userId);
	Task<bool> IsInRoleAsync(long userId, string role);

	Task<ErrorOr<long>> CreateUserAsync(
		string userName,
		string? phoneNumber,
		string? email,
		string? password = null);

	Task<ErrorOr<Success>> AddToRoleAsync(long userId, string role);
	Task<ErrorOr<Success>> UpdateUserAsync(long userId, Action<IdentityUserDto> update);

	Task<ErrorOr<Success>> SetPasswordAsync(long userId, string password);
	Task<ErrorOr<Success>> RemovePasswordAsync(long userId);
	Task<bool> CheckPasswordAsync(long userId, string password);

	Task<ErrorOr<Success>> SetPhoneVerifiedAsync(long userId);
	Task<ErrorOr<Success>> RecordLoginAsync(long userId, string provider);
}

public record IdentityUserDto(
	long Id,
	string UserName,
	string? Email,
	string? PhoneNumber,
	string? FirstName,
	string? LastName,
	string? FullName,
	string? AvatarUrl,
	bool HasPassword,
	bool IsPhoneVerified,
	bool IsActive,
	bool IsDeleted,
	DateTime? LastLoginAtUtc,
	string? LastLoginProvider);