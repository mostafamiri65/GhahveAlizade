using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Commerce.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Commerce.Queries.GetOrdersList;

public class GetOrdersListQueryHandler
	: IRequestHandler<GetOrdersListQuery, ErrorOr<PaginatedList<OrderListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetOrdersListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<OrderListItemDto>>> Handle(
		GetOrdersListQuery request, CancellationToken ct)
	{
		var query = _context.Orders
			.AsNoTracking()
			.Include(o => o.Customer)
			.Include(o => o.Items)
			.Where(o => !o.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(o =>
				o.OrderNumber.Contains(s) ||
				(o.Customer != null && (
					(o.Customer.FirstName != null && o.Customer.FirstName.Contains(s)) ||
					(o.Customer.LastName != null && o.Customer.LastName.Contains(s)) ||
					(o.Customer.PhoneNumber != null && o.Customer.PhoneNumber.Contains(s)) ||
					(o.Customer.Email != null && o.Customer.Email.Contains(s)))));
		}

		if (request.Status.HasValue)
			query = query.Where(o => o.Status == request.Status.Value);

		if (request.FromDate.HasValue)
			query = query.Where(o => o.CreatedAtUtc >= request.FromDate.Value);

		if (request.ToDate.HasValue)
			query = query.Where(o => o.CreatedAtUtc <= request.ToDate.Value);

		if (request.MinTotal.HasValue)
			query = query.Where(o => o.Total >= request.MinTotal.Value);

		if (request.MaxTotal.HasValue)
			query = query.Where(o => o.Total <= request.MaxTotal.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("total", "asc") => query.OrderBy(o => o.Total),
			("total", _) => query.OrderByDescending(o => o.Total),
			("status", "asc") => query.OrderBy(o => o.Status),
			("status", _) => query.OrderByDescending(o => o.Status),
			("createdat", "asc") => query.OrderBy(o => o.CreatedAtUtc),
			_ => query.OrderByDescending(o => o.CreatedAtUtc)
		};

		return await PaginatedList<OrderListItemDto>.CreateAsync(
			query.ProjectToType<OrderListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}