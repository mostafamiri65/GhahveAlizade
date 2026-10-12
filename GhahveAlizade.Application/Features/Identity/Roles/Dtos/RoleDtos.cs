namespace GhahveAlizade.Application.Features.Identity.Roles.Dtos;

public class RoleDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public bool IsSystemRole { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public int UserCount { get; set; }
	public int PermissionCount { get; set; }
}

public class RolePermissionGroupDto
{
	public string Group { get; set; } = string.Empty;
	public List<PermissionItemDto> Permissions { get; set; } = new();
}

public class PermissionItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string DisplayName { get; set; } = string.Empty;
	public bool IsAssigned { get; set; }
}