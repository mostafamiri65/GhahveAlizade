using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetBranchesList;

public class GetBranchesListQueryHandler
	: IRequestHandler<GetBranchesListQuery, ErrorOr<PaginatedList<BranchListItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBranchesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<BranchListItemDto>>> Handle(
		GetBranchesListQuery request, CancellationToken ct)
	{
		var query = _context.Branches
			.AsNoTracking()
			.Where(b => !b.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(b =>
				b.Name.Contains(s) ||
				b.Code.Contains(s) ||
				(b.City != null && b.City.Contains(s)) ||
				(b.Phone != null && b.Phone.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(b => b.IsActive == request.IsActive.Value);

		query = (request.SortBy?.ToLower(), request.SortDir?.ToLower()) switch
		{
			("name", "desc") => query.OrderByDescending(b => b.Name),
			("name", _) => query.OrderBy(b => b.Name),
			("createdat", "desc") => query.OrderByDescending(b => b.CreatedAtUtc),
			("createdat", _) => query.OrderBy(b => b.CreatedAtUtc),
			_ => query.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
		};

		return await PaginatedList<BranchListItemDto>.CreateAsync(
			query.ProjectToType<BranchListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}