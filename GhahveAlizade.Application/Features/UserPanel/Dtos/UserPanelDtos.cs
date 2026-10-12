namespace GhahveAlizade.Application.Features.UserPanel.Dtos;

public class MyProfileDto
{
	public long? CustomerId { get; set; }
	public long UserId { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Email { get; set; }
	public string? PhoneNumber { get; set; }
	public string? NationalCode { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string? Gender { get; set; }
	public string? AvatarUrl { get; set; }
	public bool AcceptsMarketing { get; set; }
	public string? Tier { get; set; }
	public int CurrentPoints { get; set; }
}

public class MyOrderListItemDto
{
	public long Id { get; set; }
	public string OrderNumber { get; set; } = string.Empty;
	public OrderStatus Status { get; set; }
	public string? StatusName { get; set; }
	public decimal Total { get; set; }
	public int ItemCount { get; set; }
	public DateTime CreatedAtUtc { get; set; }
	public DateTime? PaidAtUtc { get; set; }
	public string? FirstItemImageUrl { get; set; }
}

public class MyAddressDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string PhoneNumber { get; set; } = string.Empty;
	public string Province { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string PostalCode { get; set; } = string.Empty;
	public string AddressLine1 { get; set; } = string.Empty;
	public string? AddressLine2 { get; set; }
	public bool IsDefault { get; set; }
}

public class MyWishlistItemDto
{
	public long ItemId { get; set; }
	public long ProductId { get; set; }
	public string ProductName { get; set; } = string.Empty;
	public string? ProductSlug { get; set; }
	public string? ImageUrl { get; set; }
	public decimal? Price { get; set; }
	public bool IsInStock { get; set; }
	public DateTime AddedAtUtc { get; set; }
}

public class MyPointDto
{
	public int CurrentPoints { get; set; }
	public int TotalEarned { get; set; }
	public int TotalRedeemed { get; set; }
	public string? Tier { get; set; }
	public List<MyPointTransactionDto> Transactions { get; set; } = new();
}

public class MyPointTransactionDto
{
	public long Id { get; set; }
	public int Points { get; set; }
	public int BalanceAfter { get; set; }
	public string? Description { get; set; }
	public DateTime CreatedAtUtc { get; set; }
}

public class MyDashboardDto
{
	public int TotalOrders { get; set; }
	public decimal TotalSpent { get; set; }
	public int PendingOrders { get; set; }
	public int WishlistCount { get; set; }
	public int CurrentPoints { get; set; }
	public string? Tier { get; set; }
	public List<MyOrderListItemDto> RecentOrders { get; set; } = new();
}
