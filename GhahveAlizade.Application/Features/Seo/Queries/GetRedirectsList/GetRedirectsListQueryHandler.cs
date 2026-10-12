using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetRedirectsList;

public class GetRedirectsListQueryHandler
	: IRequestHandler<GetRedirectsListQuery, ErrorOr<PaginatedList<RedirectListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetRedirectsListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<RedirectListItemDto>>> Handle(
		GetRedirectsListQuery request, CancellationToken ct)
	{
		var query = _context.Redirects.AsNoTracking().Where(r => !r.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(r => r.FromPath.Contains(s) || r.ToPath.Contains(s));
		}

		if (request.IsActive.HasValue)
			query = query.Where(r => r.IsActive == request.IsActive.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("hitcount", "desc") => query.OrderByDescending(r => r.HitCount),
			("hitcount", _) => query.OrderBy(r => r.HitCount),
			("createdat", "asc") => query.OrderBy(r => r.CreatedAtUtc),
			_ => query.OrderByDescending(r => r.CreatedAtUtc)
		};

		return await PaginatedList<RedirectListItemDto>.CreateAsync(
			query.ProjectToType<RedirectListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}