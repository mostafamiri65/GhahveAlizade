using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Reports.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Reports.Queries.GetRecentOrders;

public class GetRecentOrdersQueryHandler
	: IRequestHandler<GetRecentOrdersQuery, ErrorOr<List<RecentOrderDto>>>
{
	private readonly IAppDbContext _context;

	public GetRecentOrdersQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<RecentOrderDto>>> Handle(
		GetRecentOrdersQuery request, CancellationToken ct)
	{
		var orders = await _context.Orders
			.AsNoTracking()
			.Include(o => o.Customer)
			.Where(o => !o.IsDeleted)
			.OrderByDescending(o => o.CreatedAtUtc)
			.Take(request.Count)
			.ToListAsync(ct);

		var result = orders.Select(o => new RecentOrderDto
		{
			Id = o.Id,
			OrderNumber = o.OrderNumber,
			CustomerName = o.Customer != null
				? $"{o.Customer.FirstName} {o.Customer.LastName}".Trim()
				: "مهمان",
			Total = o.Total,
			StatusName = GetStatusName(o.Status),
			StatusClass = GetStatusClass(o.Status),
			CreatedAtUtc = o.CreatedAtUtc
		}).ToList();

		return result;
	}

	private static string GetStatusName(Domain.Enums.OrderStatus s) => s switch
	{
		Domain.Enums.OrderStatus.Pending => "در انتظار",
		Domain.Enums.OrderStatus.AwaitingPayment => "منتظر پرداخت",
		Domain.Enums.OrderStatus.Paid => "پرداخت شده",
		Domain.Enums.OrderStatus.Processing => "در حال پردازش",
		Domain.Enums.OrderStatus.ReadyToShip => "آماده ارسال",
		Domain.Enums.OrderStatus.Shipped => "ارسال شده",
		Domain.Enums.OrderStatus.Delivered => "تحویل شده",
		Domain.Enums.OrderStatus.Cancelled => "لغو شده",
		Domain.Enums.OrderStatus.Refunded => "مرجوع شده",
		Domain.Enums.OrderStatus.Returned => "بازگشت",
		_ => s.ToString()
	};

	private static string GetStatusClass(Domain.Enums.OrderStatus s) => s switch
	{
		Domain.Enums.OrderStatus.Pending => "bg-secondary",
		Domain.Enums.OrderStatus.AwaitingPayment => "bg-warning text-dark",
		Domain.Enums.OrderStatus.Paid => "bg-success",
		Domain.Enums.OrderStatus.Processing => "bg-info text-dark",
		Domain.Enums.OrderStatus.ReadyToShip => "bg-primary",
		Domain.Enums.OrderStatus.Shipped => "bg-info",
		Domain.Enums.OrderStatus.Delivered => "bg-success",
		Domain.Enums.OrderStatus.Cancelled => "bg-danger",
		Domain.Enums.OrderStatus.Refunded => "bg-dark",
		Domain.Enums.OrderStatus.Returned => "bg-dark",
		_ => "bg-secondary"
	};
}