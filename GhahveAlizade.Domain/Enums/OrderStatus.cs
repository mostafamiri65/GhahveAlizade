namespace GhahveAlizade.Domain.Enums;

public enum OrderStatus
{
	Pending = 0,
	AwaitingPayment = 1,
	Paid = 2,
	Processing = 3,
	ReadyToShip = 4,
	Shipped = 5,
	Delivered = 6,
	Cancelled = 7,
	Refunded = 8,
	Returned = 9
}