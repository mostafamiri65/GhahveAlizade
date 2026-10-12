using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Marketing.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Marketing.Queries.GetBannersList;

public class GetBannersListQueryHandler
	: IRequestHandler<GetBannersListQuery, ErrorOr<PaginatedList<BannerListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBannersListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<BannerListItemDto>>> Handle(
		GetBannersListQuery request, CancellationToken ct)
	{
		var query = _context.Banners.AsNoTracking().Where(b => !b.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(b =>
				b.Title.Contains(s) ||
				(b.Subtitle != null && b.Subtitle.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(b => b.IsActive == request.IsActive.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("title", "desc") => query.OrderByDescending(b => b.Title),
			("title", _) => query.OrderBy(b => b.Title),
			("createdat", "desc") => query.OrderByDescending(b => b.CreatedAtUtc),
			("createdat", _) => query.OrderBy(b => b.CreatedAtUtc),
			("clickcount", "desc") => query.OrderByDescending(b => b.ClickCount),
			("viewcount", "desc") => query.OrderByDescending(b => b.ViewCount),
			_ => query.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Title)
		};

		return await PaginatedList<BannerListItemDto>.CreateAsync(
			query.ProjectToType<BannerListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}