using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetTopCustomers;

public class GetTopCustomersQueryHandler
	: IRequestHandler<GetTopCustomersQuery, ErrorOr<List<TopCustomerDto>>>
{
	private readonly IAppDbContext _context;

	public GetTopCustomersQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<TopCustomerDto>>> Handle(
		GetTopCustomersQuery request, CancellationToken ct)
	{
		var paidStatuses = new[]
		{
			OrderStatus.Paid, OrderStatus.Processing, OrderStatus.ReadyToShip,
			OrderStatus.Shipped, OrderStatus.Delivered
		};

		var result = await _context.Customers
			.Where(c => !c.IsDeleted)
			.OrderByDescending(c => c.TotalSpent)
			.Take(request.Count)
			.Select(c => new TopCustomerDto
			{
				CustomerId = c.Id,
				FullName = (c.FirstName ?? "") + " " + (c.LastName ?? ""),
				PhoneNumber = c.PhoneNumber,
				OrderCount = c.TotalOrders,
				TotalSpent = c.TotalSpent
			})
			.ToListAsync(ct);

		return result;
	}
}