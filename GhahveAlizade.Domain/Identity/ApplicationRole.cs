using Microsoft.AspNetCore.Identity;

namespace GhahveAlizade.Domain.Identity;

public class ApplicationRole : IdentityRole<long>
{
	public string? Description { get; set; }
	public bool IsSystemRole { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public bool IsDeleted { get; set; } = false;

}