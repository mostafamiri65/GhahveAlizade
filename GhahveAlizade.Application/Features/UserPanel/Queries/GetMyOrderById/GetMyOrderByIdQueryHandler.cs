using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrderById;

public class GetMyOrderByIdQueryHandler
	: IRequestHandler<GetMyOrderByIdQuery, ErrorOr<OrderDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetMyOrderByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<OrderDetailDto>> Handle(GetMyOrderByIdQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var order = await _context.Orders
			.AsNoTracking()
			.Include(o => o.Items)
			.Include(o => o.Addresses)
			.Include(o => o.Payments)
			.Include(o => o.Shipments)
			.Where(o => o.Id == request.OrderId
				&& !o.IsDeleted
				&& o.UserId == userIdStr) // جلوگیری از IDOR
			.FirstOrDefaultAsync(ct);

		if (order is null)
			return Error.NotFound("Order.NotFound", "سفارش یافت نشد یا به شما تعلق ندارد.");

		var dto = order.Adapt<OrderDetailDto>();
		dto.AdminNotes = null; // یادداشت ادمین هرگز به کاربر نمایش داده نمی‌شود
		return dto;
	}
}
