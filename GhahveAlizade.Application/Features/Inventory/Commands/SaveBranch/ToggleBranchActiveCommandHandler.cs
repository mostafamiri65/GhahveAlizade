using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class ToggleBranchActiveCommandHandler
	: IRequestHandler<ToggleBranchActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleBranchActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleBranchActiveCommand request, CancellationToken ct)
	{
		var branch = await _context.Branches
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (branch is null)
			return Error.NotFound("Branch.NotFound", "شعبه یافت نشد.");

		branch.IsActive = !branch.IsActive;
		await _context.SaveChangesAsync(ct);
		return branch.IsActive;
	}
}