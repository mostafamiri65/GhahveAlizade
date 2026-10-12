using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class UpdateBranchCommandHandler
	: IRequestHandler<UpdateBranchCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateBranchCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateBranchCommand request, CancellationToken ct)
	{
		var branch = await _context.Branches
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (branch is null)
			return Error.NotFound("Branch.NotFound", "شعبه یافت نشد.");

		var code = request.Code.Trim().ToUpperInvariant();

		if (await _context.Branches.AnyAsync(
			b => b.Code == code && b.Id != branch.Id && !b.IsDeleted, ct))
			return Error.Conflict("Branch.CodeExists", "این کد شعبه قبلاً استفاده شده است.");

		if (request.IsDefault && !branch.IsDefault)
		{
			var all = await _context.Branches.Where(b => b.IsDefault).ToListAsync(ct);
			foreach (var b in all) b.IsDefault = false;
		}

		branch.Name = request.Name.Trim();
		branch.Code = code;
		branch.Address = request.Address?.Trim();
		branch.City = request.City?.Trim();
		branch.Province = request.Province?.Trim();
		branch.PostalCode = request.PostalCode?.Trim();
		branch.Phone = request.Phone?.Trim();
		branch.Latitude = request.Latitude;
		branch.Longitude = request.Longitude;
		branch.WorkingHours = request.WorkingHours?.Trim();
		branch.IsActive = request.IsActive;
		branch.IsDefault = request.IsDefault;
		branch.DisplayOrder = request.DisplayOrder;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
