using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.UserPanel.Dtos;
using GhahveAlizade.Domain.Commerce;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.UserPanel.Queries.GetMyOrders;

public class GetMyOrdersQueryHandler
	: IRequestHandler<GetMyOrdersQuery, ErrorOr<PaginatedList<MyOrderListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetMyOrdersQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<MyOrderListItemDto>>> Handle(
		GetMyOrdersQuery request, CancellationToken ct)
	{
		var userIdStr = request.UserId;

		var query = _context.Orders
			.AsNoTracking()
			.Include(o => o.Items)
			.Where(o => !o.IsDeleted && o.UserId == userIdStr)
			.OrderByDescending(o => o.CreatedAtUtc);

		if (request.Status.HasValue)
			query = (IOrderedQueryable<Order>)query.Where(o => o.Status == request.Status.Value);

		return await PaginatedList<MyOrderListItemDto>.CreateAsync(
			query.ProjectToType<MyOrderListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}
