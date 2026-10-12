using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Inventory;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class CreateBranchCommandHandler
	: IRequestHandler<CreateBranchCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;

	public CreateBranchCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateBranchCommand request, CancellationToken ct)
	{
		var code = request.Code.Trim().ToUpperInvariant();

		if (await _context.Branches.AnyAsync(b => b.Code == code && !b.IsDeleted, ct))
			return Error.Conflict("Branch.CodeExists", "این کد شعبه قبلاً استفاده شده است.");

		// اگه IsDefault=true، بقیه رو false کن
		if (request.IsDefault)
		{
			var all = await _context.Branches.Where(b => b.IsDefault).ToListAsync(ct);
			foreach (var b in all) b.IsDefault = false;
		}

		var branch = new Branch
		{
			Name = request.Name.Trim(),
			Code = code,
			Address = request.Address?.Trim(),
			City = request.City?.Trim(),
			Province = request.Province?.Trim(),
			PostalCode = request.PostalCode?.Trim(),
			Phone = request.Phone?.Trim(),
			Latitude = request.Latitude,
			Longitude = request.Longitude,
			WorkingHours = request.WorkingHours?.Trim(),
			IsActive = request.IsActive,
			IsDefault = request.IsDefault,
			DisplayOrder = request.DisplayOrder
		};

		_context.Branches.Add(branch);
		await _context.SaveChangesAsync(ct);
		return branch.Id;
	}
}
