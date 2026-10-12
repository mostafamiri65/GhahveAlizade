using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetWarehousesList;

public class GetWarehousesListQueryHandler
	: IRequestHandler<GetWarehousesListQuery, ErrorOr<PaginatedList<WarehouseListItemDto>>>
{
	private readonly IAppDbContext _context;
	public GetWarehousesListQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<PaginatedList<WarehouseListItemDto>>> Handle(
		GetWarehousesListQuery request, CancellationToken ct)
	{
		var query = _context.Warehouses.AsNoTracking().Where(w => !w.IsDeleted);

		if (!string.IsNullOrWhiteSpace(request.Search))
		{
			var s = request.Search.Trim();
			query = query.Where(w => w.Name.Contains(s) || w.Code.Contains(s) ||
				(w.City != null && w.City.Contains(s)));
		}

		if (request.IsActive.HasValue)
			query = query.Where(w => w.IsActive == request.IsActive.Value);

		query = request.SortDir?.ToLower() == "desc"
			? query.OrderByDescending(w => w.Name)
			: query.OrderBy(w => w.Name);

		return await PaginatedList<WarehouseListItemDto>.CreateAsync(
			query.ProjectToType<WarehouseListItemDto>(),
			request.PageNumber, request.PageSize, ct);
	}
}