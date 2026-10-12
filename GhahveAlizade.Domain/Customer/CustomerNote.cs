using GhahveAlizade.Domain.Common;

namespace GhahveAlizade.Domain.Customer;

public class CustomerNote : BaseEntity
{
	public long CustomerId { get; set; }
	public Customer Customer { get; set; } = default!;

	public string Content { get; set; } = default!;
	public string? Category { get; set; }
	public bool IsInternal { get; set; } = true;
}