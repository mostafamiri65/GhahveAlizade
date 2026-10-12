using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Commerce.Commands.ChangeOrderStatus;

public class ChangeOrderStatusCommandHandler
	: IRequestHandler<ChangeOrderStatusCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public ChangeOrderStatusCommandHandler(IAppDbContext context, IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<Success>> Handle(
		ChangeOrderStatusCommand request, CancellationToken ct)
	{
		var order = await _context.Orders
			.FirstOrDefaultAsync(o => o.Id == request.OrderId && !o.IsDeleted, ct);

		if (order is null) return Error.NotFound("Order.NotFound", "سفارش یافت نشد.");

		var oldStatus = order.Status;

		// اعتبارسنجی انتقال بین وضعیت‌ها
		if (!IsValidTransition(oldStatus, request.NewStatus))
			return Error.Validation("Order.InvalidTransition",
				$"انتقال از وضعیت '{oldStatus}' به '{request.NewStatus}' مجاز نیست.");

		order.Status = request.NewStatus;

		// ثبت timestamp مناسب
		switch (request.NewStatus)
		{
			case OrderStatus.Paid when !order.PaidAtUtc.HasValue:
				order.PaidAtUtc = _dateTime.UtcNow;
				break;
			case OrderStatus.Shipped when !order.ShippedAtUtc.HasValue:
				order.ShippedAtUtc = _dateTime.UtcNow;
				break;
			case OrderStatus.Delivered when !order.DeliveredAtUtc.HasValue:
				order.DeliveredAtUtc = _dateTime.UtcNow;
				break;
			case OrderStatus.Cancelled when !order.CancelledAtUtc.HasValue:
				order.CancelledAtUtc = _dateTime.UtcNow;
				break;
		}

		if (!string.IsNullOrWhiteSpace(request.Note))
		{
			var note = string.IsNullOrEmpty(order.AdminNotes)
				? ""
				: order.AdminNotes + Environment.NewLine;
			note += $"[{_dateTime.UtcNow:yyyy-MM-dd HH:mm}] تغییر وضعیت از {oldStatus} به {request.NewStatus}: {request.Note}";
			order.AdminNotes = note;
		}

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}

	private static bool IsValidTransition(OrderStatus from, OrderStatus to)
	{
		if (from == to) return false;
		return from switch
		{
			OrderStatus.Pending => to is OrderStatus.AwaitingPayment
				or OrderStatus.Paid or OrderStatus.Cancelled,
			OrderStatus.AwaitingPayment => to is OrderStatus.Paid
				or OrderStatus.Cancelled or OrderStatus.Pending,
			OrderStatus.Paid => to is OrderStatus.Processing
				or OrderStatus.Cancelled or OrderStatus.Refunded,
			OrderStatus.Processing => to is OrderStatus.ReadyToShip
				or OrderStatus.Cancelled,
			OrderStatus.ReadyToShip => to is OrderStatus.Shipped,
			OrderStatus.Shipped => to is OrderStatus.Delivered
				or OrderStatus.Returned,
			OrderStatus.Delivered => to is OrderStatus.Returned,
			OrderStatus.Cancelled => false,
			OrderStatus.Refunded => false,
			OrderStatus.Returned => false,
			_ => false
		};
	}
}