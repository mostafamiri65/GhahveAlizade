using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Coffee.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Queries.GetCoffeeProcesses;

public class GetCoffeeProcessesListQueryHandler
	: IRequestHandler<GetCoffeeProcessesListQuery, ErrorOr<PaginatedList<CoffeeProcessListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetCoffeeProcessesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<CoffeeProcessListItemDto>>> Handle(
		GetCoffeeProcessesListQuery request, CancellationToken ct)
	{
		var query = _context.CoffeeProcesses.AsNoTracking().Where(p => !p.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(p => p.Name.Contains(s) || p.Slug.Contains(s));
		}

		if (request.IsActive.HasValue)
			query = query.Where(p => p.IsActive == request.IsActive.Value);

		query = request.SortBy?.ToLower() switch
		{
			"name" => request.SortDir == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
			"createdat" => request.SortDir == "desc" ? query.OrderByDescending(p => p.CreatedAtUtc) : query.OrderBy(p => p.CreatedAtUtc),
			_ => query.OrderBy(p => p.DisplayOrder).ThenBy(p => p.Name)
		};

		return await PaginatedList<CoffeeProcessListItemDto>.CreateAsync(
			query.ProjectToType<CoffeeProcessListItemDto>(), request.PageNumber, request.PageSize, ct);
	}
}
