namespace GhahveAlizade.Application.Features.Identity.Users.Dtos;

public class UserListItemDto
{
	public long Id { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? FullName { get; set; }
	public bool IsActive { get; set; }
	public bool IsPhoneVerified { get; set; }
	public bool HasPassword { get; set; }
	public DateTime? LastLoginAtUtc { get; set; }
	public string? LastLoginProvider { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public List<string> Roles { get; set; } = new();
}

public class UserDetailDto : UserListItemDto
{
	public string? AvatarUrl { get; set; }
	public string? NationalCode { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string? Gender { get; set; }
}