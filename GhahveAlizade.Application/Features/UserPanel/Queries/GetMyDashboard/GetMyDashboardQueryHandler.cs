using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using GhahveAlizade.Domain.Enums;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyDashboard;

public class GetMyDashboardQueryHandler : IRequestHandler<GetMyDashboardQuery, ErrorOr<MyDashboardDto>>
{
	private readonly IAppDbContext _context;

	public GetMyDashboardQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<MyDashboardDto>> Handle(GetMyDashboardQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId.ToString();

		var ordersQuery = _context.Orders.AsNoTracking()
			.Where(o => !o.IsDeleted && o.UserId == userIdStr);

		var totalOrders = await ordersQuery.CountAsync(ct);
		var totalSpent = await ordersQuery
			.Where(o => o.Status != OrderStatus.Cancelled)
			.SumAsync(o => (decimal?)o.Total, ct) ?? 0m;
		var pendingOrders = await ordersQuery
			.CountAsync(o => o.Status is OrderStatus.Pending or OrderStatus.AwaitingPayment
				or OrderStatus.Paid or OrderStatus.Processing, ct);

		var wishlistCount = await _context.WishlistItems.AsNoTracking()
			.CountAsync(i => !i.IsDeleted
				&& i.Wishlist != null && !i.Wishlist.IsDeleted
				&& i.Wishlist.UserId == userIdStr, ct);

		var pointAccount = await _context.CustomerPointAccounts.AsNoTracking()
			.Include(p => p.Customer)
			.Where(p => !p.IsDeleted && p.Customer != null && p.Customer.UserId == userIdStr)
			.FirstOrDefaultAsync(ct);

		var recentOrders = await ordersQuery
			.Include(o => o.Items)
			.Take(5)
			.ProjectToType<MyOrderListItemDto>()
			.ToListAsync(ct);

		return new MyDashboardDto
		{
			TotalOrders = totalOrders,
			TotalSpent = totalSpent,
			PendingOrders = pendingOrders,
			WishlistCount = wishlistCount,
			CurrentPoints = pointAccount?.CurrentPoints ?? 0,
			Tier = pointAccount?.Tier,
			RecentOrders = recentOrders
		};
	}
}
