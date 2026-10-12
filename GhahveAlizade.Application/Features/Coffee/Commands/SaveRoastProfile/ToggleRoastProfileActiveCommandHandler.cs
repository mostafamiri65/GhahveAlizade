using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public class ToggleRoastProfileActiveCommandHandler
	: IRequestHandler<ToggleRoastProfileActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleRoastProfileActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleRoastProfileActiveCommand request, CancellationToken ct)
	{
		var roast = await _context.RoastProfiles
			.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, ct);

		if (roast is null)
			return Error.NotFound("RoastProfile.NotFound", "پروفایل رست یافت نشد.");

		roast.IsActive = !roast.IsActive;
		await _context.SaveChangesAsync(ct);
		return roast.IsActive;
	}
}