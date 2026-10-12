using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Inventory.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Inventory.Queries.GetBranchById;

public class GetBranchByIdQueryHandler
	: IRequestHandler<GetBranchByIdQuery, ErrorOr<BranchDto>>
{
	private readonly IAppDbContext _context;

	public GetBranchByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BranchDto>> Handle(GetBranchByIdQuery request, CancellationToken ct)
	{
		var branch = await _context.Branches
			.AsNoTracking()
			.Where(b => b.Id == request.Id && !b.IsDeleted)
			.ProjectToType<BranchDto>()
			.FirstOrDefaultAsync(ct);

		if (branch is null)
			return Error.NotFound("Branch.NotFound", "شعبه یافت نشد.");

		return branch;
	}
}