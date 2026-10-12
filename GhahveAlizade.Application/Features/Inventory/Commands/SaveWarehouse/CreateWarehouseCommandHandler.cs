using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Inventory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveWarehouse;

public class CreateWarehouseCommandHandler
	: IRequestHandler<CreateWarehouseCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateWarehouseCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateWarehouseCommand request, CancellationToken ct)
	{
		var code = request.Code.Trim().ToUpperInvariant();
		if (await _context.Warehouses.AnyAsync(w => w.Code == code && !w.IsDeleted, ct))
			return Error.Conflict("Warehouse.CodeExists", "این کد انبار قبلاً استفاده شده است.");

		// اگه IsDefault=true، بقیه رو false کن
		if (request.IsDefault)
		{
			var all = await _context.Warehouses.Where(w => w.IsDefault).ToListAsync(ct);
			foreach (var w in all) w.IsDefault = false;
		}

		var warehouse = new Warehouse
		{
			Name = request.Name.Trim(),
			Code = code,
			Address = request.Address?.Trim(),
			City = request.City?.Trim(),
			Province = request.Province?.Trim(),
			PostalCode = request.PostalCode?.Trim(),
			Phone = request.Phone?.Trim(),
			ManagerName = request.ManagerName?.Trim(),
			IsActive = request.IsActive,
			IsDefault = request.IsDefault
		};

		_context.Warehouses.Add(warehouse);
		await _context.SaveChangesAsync(ct);
		return warehouse.Id;
	}
}
