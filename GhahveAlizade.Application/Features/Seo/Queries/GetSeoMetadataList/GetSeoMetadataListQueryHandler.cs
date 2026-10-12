using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Seo.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataList;

public class GetSeoMetadataListQueryHandler
	: IRequestHandler<GetSeoMetadataListQuery, ErrorOr<PaginatedList<SeoMetadataListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetSeoMetadataListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<SeoMetadataListItemDto>>> Handle(
		GetSeoMetadataListQuery request, CancellationToken ct)
	{
		var query = _context.SeoMetadata.AsNoTracking().Where(s => !s.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(x =>
				(x.MetaTitle != null && x.MetaTitle.Contains(s)) ||
				(x.MetaDescription != null && x.MetaDescription.Contains(s)));
		}

		if (request.EntityType.HasValue)
			query = query.Where(s => s.EntityType == request.EntityType.Value);

		query = query.OrderByDescending(s => s.CreatedAtUtc);

		return await PaginatedList<SeoMetadataListItemDto>.CreateAsync(
			query.ProjectToType<SeoMetadataListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}