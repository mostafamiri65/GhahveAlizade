using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Customers.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Customers.Queries.GetCustomersList;

public class GetCustomersListQueryHandler
	: IRequestHandler<GetCustomersListQuery, ErrorOr<PaginatedList<CustomerListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetCustomersListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<CustomerListItemDto>>> Handle(
		GetCustomersListQuery request, CancellationToken ct)
	{
		var query = _context.Customers
			.AsNoTracking()
			.Include(c => c.CustomerGroup)
			.Where(c => !c.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(c =>
				(c.FirstName != null && c.FirstName.Contains(s)) ||
				(c.LastName != null && c.LastName.Contains(s)) ||
				(c.PhoneNumber != null && c.PhoneNumber.Contains(s)) ||
				(c.Email != null && c.Email.Contains(s)) ||
				(c.NationalCode != null && c.NationalCode.Contains(s)));
		}

		if (request.CustomerGroupId.HasValue)
			query = query.Where(c => c.CustomerGroupId == request.CustomerGroupId.Value);

		if (request.IsActive.HasValue)
			query = query.Where(c => c.IsActive == request.IsActive.Value);

		if (request.IsVerified.HasValue)
			query = query.Where(c => c.IsVerified == request.IsVerified.Value);

		if (request.HasOrders.HasValue)
		{
			query = request.HasOrders.Value
				? query.Where(c => c.TotalOrders > 0)
				: query.Where(c => c.TotalOrders == 0);
		}

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("fullname", "asc") => query.OrderBy(c => c.FirstName).ThenBy(c => c.LastName),
			("fullname", _) => query.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName),
			("totalspent", "desc") => query.OrderByDescending(c => c.TotalSpent),
			("totalspent", _) => query.OrderBy(c => c.TotalSpent),
			("totalorders", "desc") => query.OrderByDescending(c => c.TotalOrders),
			("totalorders", _) => query.OrderBy(c => c.TotalOrders),
			("lastorder", "desc") => query.OrderByDescending(c => c.LastOrderAtUtc),
			("lastorder", _) => query.OrderBy(c => c.LastOrderAtUtc),
			("createdat", "asc") => query.OrderBy(c => c.CreatedAtUtc),
			_ => query.OrderByDescending(c => c.CreatedAtUtc)
		};

		return await PaginatedList<CustomerListItemDto>.CreateAsync(
			query.ProjectToType<CustomerListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}