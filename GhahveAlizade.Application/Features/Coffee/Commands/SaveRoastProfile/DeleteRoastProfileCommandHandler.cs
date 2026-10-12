using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public class DeleteRoastProfileCommandHandler
	: IRequestHandler<DeleteRoastProfileCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteRoastProfileCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteRoastProfileCommand request, CancellationToken ct)
	{
		var roast = await _context.RoastProfiles
			.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, ct);

		if (roast is null)
			return Error.NotFound("RoastProfile.NotFound", "پروفایل رست یافت نشد.");

		var isUsed = await _context.Products
			.AnyAsync(p => p.RoastProfileId == roast.Id && !p.IsDeleted, ct);

		if (isUsed)
			return Error.Conflict("RoastProfile.InUse", "این پروفایل در محصولات استفاده شده است.");

		_context.RoastProfiles.Remove(roast);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
