using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public class UpdateWarehouseCommandHandler
	: IRequestHandler<UpdateWarehouseCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateWarehouseCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateWarehouseCommand request, CancellationToken ct)
	{
		var w = await _context.Warehouses
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (w is null) return Error.NotFound("Warehouse.NotFound", "انبار یافت نشد.");

		var code = request.Code.Trim().ToUpperInvariant();
		if (await _context.Warehouses.AnyAsync(
			x => x.Code == code && x.Id != w.Id && !x.IsDeleted, ct))
			return Error.Conflict("Warehouse.CodeExists", "این کد انبار قبلاً استفاده شده است.");

		if (request.IsDefault && !w.IsDefault)
		{
			var all = await _context.Warehouses.Where(x => x.IsDefault).ToListAsync(ct);
			foreach (var other in all) other.IsDefault = false;
		}

		w.Name = request.Name.Trim();
		w.Code = code;
		w.Address = request.Address?.Trim();
		w.City = request.City?.Trim();
		w.Province = request.Province?.Trim();
		w.PostalCode = request.PostalCode?.Trim();
		w.Phone = request.Phone?.Trim();
		w.ManagerName = request.ManagerName?.Trim();
		w.IsActive = request.IsActive;
		w.IsDefault = request.IsDefault;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
