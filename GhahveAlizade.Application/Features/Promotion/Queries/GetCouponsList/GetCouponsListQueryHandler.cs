using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Promotion.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Promotion.Queries.GetCouponsList;

public class GetCouponsListQueryHandler
	: IRequestHandler<GetCouponsListQuery, ErrorOr<PaginatedList<CouponListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetCouponsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<CouponListItemDto>>> Handle(
		GetCouponsListQuery request, CancellationToken ct)
	{
		var query = _context.Coupons.AsNoTracking().Where(c => !c.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(c => c.Code.Contains(s) ||
				(c.Description != null && c.Description.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(c => c.IsActive == request.IsActive.Value);

		query = request.SortBy?.ToLower() switch
		{
			"code" => request.SortDir == "desc" ? query.OrderByDescending(c => c.Code) : query.OrderBy(c => c.Code),
			"usedcount" => request.SortDir == "desc" ? query.OrderByDescending(c => c.UsedCount) : query.OrderBy(c => c.UsedCount),
			"createdat" => request.SortDir == "desc" ? query.OrderByDescending(c => c.CreatedAtUtc) : query.OrderBy(c => c.CreatedAtUtc),
			_ => query.OrderByDescending(c => c.CreatedAtUtc)
		};

		return await PaginatedList<CouponListItemDto>.CreateAsync(
			query.ProjectToType<CouponListItemDto>(), request.PageNumber, request.PageSize, ct);
	}
}