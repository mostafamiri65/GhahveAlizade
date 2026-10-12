using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetWarehouseById;

public class GetWarehouseByIdQueryHandler
	: IRequestHandler<GetWarehouseByIdQuery, ErrorOr<WarehouseDto>>
{
	private readonly IAppDbContext _context;
	public GetWarehouseByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<WarehouseDto>> Handle(GetWarehouseByIdQuery request, CancellationToken ct)
	{
		var w = await _context.Warehouses.AsNoTracking()
			.Where(x => x.Id == request.Id && !x.IsDeleted)
			.ProjectToType<WarehouseDto>()
			.FirstOrDefaultAsync(ct);

		if (w is null) return Error.NotFound("Warehouse.NotFound", "انبار یافت نشد.");
		return w;
	}
}