using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Domain.Identity;

public class ApplicationUser : IdentityUser<long>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? AvatarUrl { get; set; }
	public string? NationalCode { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string? Gender { get; set; }

	public bool IsActive { get; set; } = true;
	public DateTime? LastLoginAtUtc { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public long? CreatedBy { get; set; }
	public DateTime? UpdatedAtUtc { get; set; }
	public long? UpdatedBy { get; set; }
	public bool IsDeleted { get; set; }

	public bool IsPhoneVerified { get; set; }
	public DateTime? PhoneVerifiedAtUtc { get; set; }
	public bool HasPassword { get; set; }  // آیا کاربر پسورد تعیین کرده
	public string? LastLoginProvider { get; set; } // "OTP" | "Password" | "Google" ...


	public string FullName => $"{FirstName} {LastName}".Trim();
}