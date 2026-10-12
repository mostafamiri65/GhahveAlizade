using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Identity;

public class Permission : BaseEntity
{
	public string Name { get; set; } = default!; // "Products.Create"
	public string DisplayName { get; set; } = default!; // "ایجاد محصول"
	public string? Description { get; set; }
	public string? Group { get; set; } // "Products", "Orders", "Users"
	public int DisplayOrder { get; set; }

	public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}