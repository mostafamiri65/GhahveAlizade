using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Commands.CancelMyOrder;

public class CancelMyOrderCommandHandler : IRequestHandler<CancelMyOrderCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public CancelMyOrderCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(CancelMyOrderCommand request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var order = await _context.Orders
			.Where(o => o.Id == request.OrderId && !o.IsDeleted && o.UserId == userIdStr)
			.FirstOrDefaultAsync(ct);

		if (order is null)
			return Error.NotFound("Order.NotFound", "سفارش یافت نشد.");

		// فقط قبل از ارسال قابل لغو است
		var cancellable = order.Status is OrderStatus.Pending
			or OrderStatus.AwaitingPayment or OrderStatus.Paid or OrderStatus.Processing;

		if (!cancellable)
			return Error.Conflict("Order.NotCancellable",
				"این سفارش در این وضعیت قابل لغو نیست. لطفاً با پشتیبانی تماس بگیرید.");

		order.Status = OrderStatus.Cancelled;
		order.CancelledAtUtc = DateTime.UtcNow;
		var reasonSuffix = string.IsNullOrWhiteSpace(request.Reason)
			? ""
			: $": {request.Reason}";
		order.CustomerNotes = string.IsNullOrWhiteSpace(order.CustomerNotes)
			? $"لغو توسط مشتری{reasonSuffix}"
			: order.CustomerNotes + $"\nلغو توسط مشتری{reasonSuffix}";
		order.UpdatedAtUtc = DateTime.UtcNow;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
