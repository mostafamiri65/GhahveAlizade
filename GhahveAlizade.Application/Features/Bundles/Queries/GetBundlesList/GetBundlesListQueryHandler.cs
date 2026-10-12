using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Bundles.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundlesList;

public class GetBundlesListQueryHandler
	: IRequestHandler<GetBundlesListQuery, ErrorOr<PaginatedList<ProductBundleListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetBundlesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<ProductBundleListItemDto>>> Handle(
		GetBundlesListQuery request, CancellationToken ct)
	{
		var query = _context.ProductBundles.AsNoTracking().Where(b => !b.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(b => b.Name.Contains(s) || b.Slug.Contains(s));
		}

		if (request.IsActive.HasValue)
			query = query.Where(b => b.IsActive == request.IsActive.Value);

		if (request.IsFeatured.HasValue)
			query = query.Where(b => b.IsFeatured == request.IsFeatured.Value);

		query = request.SortDir?.ToLower() == "asc"
			? query.OrderBy(b => b.CreatedAtUtc)
			: query.OrderByDescending(b => b.CreatedAtUtc);

		return await PaginatedList<ProductBundleListItemDto>.CreateAsync(
			query.ProjectToType<ProductBundleListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}