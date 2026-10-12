using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Identity;

public class RolePermission : BaseEntity
{
	public long RoleId { get; set; } = default!;
	public ApplicationRole Role { get; set; } = default!;

	public long PermissionId { get; set; }
	public Permission Permission { get; set; } = default!;
}