using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetRoastProfiles;

public class GetRoastProfilesListQueryHandler
	: IRequestHandler<GetRoastProfilesListQuery, ErrorOr<PaginatedList<RoastProfileListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetRoastProfilesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<RoastProfileListItemDto>>> Handle(
		GetRoastProfilesListQuery request, CancellationToken ct)
	{
		var query = _context.RoastProfiles.AsNoTracking().Where(r => !r.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(r => r.Name.Contains(s) || r.Slug.Contains(s));
		}

		if (request.IsActive.HasValue)
			query = query.Where(r => r.IsActive == request.IsActive.Value);

		query = request.SortBy?.ToLower() switch
		{
			"name" => request.SortDir == "desc" ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
			"roastlevel" => request.SortDir == "desc" ? query.OrderByDescending(r => r.RoastLevel) : query.OrderBy(r => r.RoastLevel),
			_ => query.OrderBy(r => r.DisplayOrder).ThenBy(r => r.Name)
		};

		return await PaginatedList<RoastProfileListItemDto>.CreateAsync(
			query.ProjectToType<RoastProfileListItemDto>(), request.PageNumber, request.PageSize, ct);
	}
}
