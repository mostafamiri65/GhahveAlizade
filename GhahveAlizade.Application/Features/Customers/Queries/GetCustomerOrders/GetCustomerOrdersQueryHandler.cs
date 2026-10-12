using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomerOrders;

public class GetCustomerOrdersQueryHandler
	: IRequestHandler<GetCustomerOrdersQuery, ErrorOr<List<OrderListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetCustomerOrdersQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<OrderListItemDto>>> Handle(
		GetCustomerOrdersQuery request, CancellationToken ct)
	{
		var orders = await _context.Orders
			.AsNoTracking()
			.Include(o => o.Customer)
			.Include(o => o.Items)
			.Where(o => o.CustomerId == request.CustomerId && !o.IsDeleted)
			.OrderByDescending(o => o.CreatedAtUtc)
			.Take(request.Take)
			.ProjectToType<OrderListItemDto>()
			.ToListAsync(ct);

		return orders;
	}
}