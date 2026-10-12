using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
public class GetWarehousesLookupQueryHandler : IRequestHandler<GetWarehousesLookupQuery, ErrorOr<List<LookupItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetWarehousesLookupQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<LookupItemDto>>> Handle(GetWarehousesLookupQuery request, CancellationToken ct)
	{
		return await _context.Warehouses
			.Where(w => !w.IsDeleted && w.IsActive)
			.OrderBy(w => w.Name)
			.Select(w => new LookupItemDto { Id = w.Id, Name = w.Name })
			.ToListAsync(ct);
	}
}