using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveRoastProfile;

public class CreateRoastProfileCommandHandler
	: IRequestHandler<CreateRoastProfileCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateRoastProfileCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateRoastProfileCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.RoastProfiles.AnyAsync(r => r.Slug == slug && !r.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var roast = new RoastProfile
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim(),
			ColorCode = request.ColorCode?.Trim(),
			RoastLevel = request.RoastLevel,
			IsActive = request.IsActive,
			DisplayOrder = request.DisplayOrder
		};

		_context.RoastProfiles.Add(roast);
		await _context.SaveChangesAsync(ct);
		return roast.Id;
	}
}
