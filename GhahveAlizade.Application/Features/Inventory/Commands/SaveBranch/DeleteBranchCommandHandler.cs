using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Commands.SaveBranch;

public class DeleteBranchCommandHandler
	: IRequestHandler<DeleteBranchCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteBranchCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBranchCommand request, CancellationToken ct)
	{
		var branch = await _context.Branches
			.FirstOrDefaultAsync(b => b.Id == request.Id && !b.IsDeleted, ct);

		if (branch is null)
			return Error.NotFound("Branch.NotFound", "شعبه یافت نشد.");

		if (branch.IsDefault)
			return Error.Conflict("Branch.IsDefault", "شعبه پیش‌فرض قابل حذف نیست.");

		_context.Branches.Remove(branch);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
