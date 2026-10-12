using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Wholesale.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Wholesale.Queries.GetWholesaleRequestsList;

public class GetWholesaleRequestsListQueryHandler
	: IRequestHandler<GetWholesaleRequestsListQuery, ErrorOr<PaginatedList<WholesaleRequestListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetWholesaleRequestsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<WholesaleRequestListItemDto>>> Handle(
		GetWholesaleRequestsListQuery request, CancellationToken ct)
	{
		var query = _context.WholesaleRequests.AsNoTracking().Where(r => !r.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(r =>
				r.CompanyName.Contains(s) ||
				r.ContactPerson.Contains(s) ||
				r.ContactPhone.Contains(s));
		}

		if (request.Status.HasValue)
			query = query.Where(r => r.Status == request.Status.Value);

		query = request.SortDir?.ToLower() == "asc"
			? query.OrderBy(r => r.CreatedAtUtc)
			: query.OrderByDescending(r => r.CreatedAtUtc);

		return await PaginatedList<WholesaleRequestListItemDto>.CreateAsync(
			query.ProjectToType<WholesaleRequestListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}