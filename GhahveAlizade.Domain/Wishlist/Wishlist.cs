using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Wishlist;

public class Wishlist : BaseEntity
{
	public string? UserId { get; set; }
	public long? CustomerId { get; set; }
	public Customer.Customer? Customer { get; set; }

	public string Name { get; set; } = "علاقه‌مندی‌ها";
	public bool IsPublic { get; set; }
	public string? ShareToken { get; set; }

	public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}