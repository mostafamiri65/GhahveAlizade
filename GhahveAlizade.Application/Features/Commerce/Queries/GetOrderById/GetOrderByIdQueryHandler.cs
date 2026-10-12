using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Commerce.Queries.GetOrderById;

public class GetOrderByIdQueryHandler
	: IRequestHandler<GetOrderByIdQuery, ErrorOr<OrderDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetOrderByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<OrderDetailDto>> Handle(GetOrderByIdQuery request, CancellationToken ct)
	{
		var order = await _context.Orders
			.AsNoTracking()
			.Include(o => o.Customer)
			.Include(o => o.Items)
			.Include(o => o.Addresses)
			.Include(o => o.Payments)
			.Include(o => o.Shipments)
			.Where(o => o.Id == request.Id && !o.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (order is null) return Error.NotFound("Order.NotFound", "سفارش یافت نشد.");

		return order.Adapt<OrderDetailDto>();
	}
}