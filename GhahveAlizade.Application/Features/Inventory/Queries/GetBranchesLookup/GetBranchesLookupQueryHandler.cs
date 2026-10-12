using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetBranchesLookup;

public class GetBranchesLookupQueryHandler
	: IRequestHandler<GetBranchesLookupQuery, ErrorOr<List<LookupItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBranchesLookupQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<LookupItemDto>>> Handle(
		GetBranchesLookupQuery request, CancellationToken ct)
	{
		var query = _context.Branches.AsNoTracking().Where(b => !b.IsDeleted);

		if (request.OnlyActive)
			query = query.Where(b => b.IsActive);

		var result = await query
			.OrderBy(b => b.DisplayOrder).ThenBy(b => b.Name)
			.Select(b => new LookupItemDto { Id = b.Id, Name = b.Name })
			.ToListAsync(ct);

		return result;
	}
}