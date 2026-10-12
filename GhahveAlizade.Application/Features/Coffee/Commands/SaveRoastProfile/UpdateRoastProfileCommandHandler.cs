using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public class UpdateRoastProfileCommandHandler
	: IRequestHandler<UpdateRoastProfileCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateRoastProfileCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateRoastProfileCommand request, CancellationToken ct)
	{
		var roast = await _context.RoastProfiles
			.FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted, ct);

		if (roast is null)
			return Error.NotFound("RoastProfile.NotFound", "پروفایل رست یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.RoastProfiles.AnyAsync(r => r.Slug == slug && r.Id != roast.Id && !r.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		roast.Name = request.Name.Trim();
		roast.Slug = slug;
		roast.Description = request.Description?.Trim();
		roast.ColorCode = request.ColorCode?.Trim();
		roast.RoastLevel = request.RoastLevel;
		roast.IsActive = request.IsActive;
		roast.DisplayOrder = request.DisplayOrder;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
