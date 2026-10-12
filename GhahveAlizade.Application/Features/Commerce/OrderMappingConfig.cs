using GhahveAlizade.Application.Features.Commerce.Dtos;
using GhahveAlizade.Domain.Commerce;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.Payment;
using GhahveAlizade.Domain.Shipping;
using Mapster;

namespace GhahveAlizade.Application.Features.Commerce;

public class OrderMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Order, OrderListItemDto>()
			.Map(dest => dest.CustomerName,
				src => src.Customer != null
					? $"{src.Customer.FirstName} {src.Customer.LastName}".Trim()
					: null)
			.Map(dest => dest.CustomerPhone,
				src => src.Customer != null ? src.Customer.PhoneNumber : null)
			.Map(dest => dest.CustomerEmail,
				src => src.Customer != null ? src.Customer.Email : null)
			.Map(dest => dest.StatusName, src => GetOrderStatusName(src.Status))
			.Map(dest => dest.ItemCount, src => src.Items.Count(i => !i.IsDeleted));

		config.NewConfig<Order, OrderDetailDto>()
			.Map(dest => dest.CustomerName,
				src => src.Customer != null
					? $"{src.Customer.FirstName} {src.Customer.LastName}".Trim()
					: null)
			.Map(dest => dest.CustomerPhone,
				src => src.Customer != null ? src.Customer.PhoneNumber : null)
			.Map(dest => dest.CustomerEmail,
				src => src.Customer != null ? src.Customer.Email : null)
			.Map(dest => dest.StatusName, src => GetOrderStatusName(src.Status))
			.Map(dest => dest.ItemCount, src => src.Items.Count(i => !i.IsDeleted));

		config.NewConfig<OrderItem, OrderItemDto>();
		config.NewConfig<OrderAddress, OrderAddressDto>();
		config.NewConfig<Payment, PaymentDto>()
			.Map(dest => dest.MethodName, src => GetPaymentMethodName(src.Method))
			.Map(dest => dest.StatusName, src => GetPaymentStatusName(src.Status));
		config.NewConfig<Shipment, ShipmentDto>()
			.Map(dest => dest.StatusName, src => GetShipmentStatusName(src.Status));
	}

	public static string GetOrderStatusName(OrderStatus s) => s switch
	{
		OrderStatus.Pending => "در انتظار",
		OrderStatus.AwaitingPayment => "منتظر پرداخت",
		OrderStatus.Paid => "پرداخت شده",
		OrderStatus.Processing => "در حال پردازش",
		OrderStatus.ReadyToShip => "آماده ارسال",
		OrderStatus.Shipped => "ارسال شده",
		OrderStatus.Delivered => "تحویل شده",
		OrderStatus.Cancelled => "لغو شده",
		OrderStatus.Refunded => "مرجوع شده",
		OrderStatus.Returned => "بازگشت داده شده",
		_ => s.ToString()
	};

	public static string GetPaymentMethodName(PaymentMethod m) => m switch
	{
		PaymentMethod.ZarinPal => "زرین‌پال",
		PaymentMethod.IDPay => "آیدی‌پی",
		PaymentMethod.CashOnDelivery => "پرداخت در محل",
		PaymentMethod.Wallet => "کیف پول",
		PaymentMethod.BankTransfer => "انتقال بانکی",
		_ => m.ToString()
	};

	public static string GetPaymentStatusName(PaymentStatus s) => s switch
	{
		PaymentStatus.Pending => "در انتظار",
		PaymentStatus.Authorized => "تایید شده",
		PaymentStatus.Paid => "پرداخت شده",
		PaymentStatus.Failed => "ناموفق",
		PaymentStatus.Refunded => "مرجوع شده",
		PaymentStatus.PartiallyRefunded => "بخشی مرجوع",
		PaymentStatus.Cancelled => "لغو شده",
		_ => s.ToString()
	};

	public static string GetShipmentStatusName(ShipmentStatus s) => s switch
	{
		ShipmentStatus.Pending => "در انتظار",
		ShipmentStatus.Preparing => "در حال آماده‌سازی",
		ShipmentStatus.ReadyToShip => "آماده ارسال",
		ShipmentStatus.InTransit => "در مسیر",
		ShipmentStatus.OutForDelivery => "در حال تحویل",
		ShipmentStatus.Delivered => "تحویل شده",
		ShipmentStatus.Failed => "ناموفق",
		ShipmentStatus.Returned => "بازگشت داده شده",
		_ => s.ToString()
	};
}