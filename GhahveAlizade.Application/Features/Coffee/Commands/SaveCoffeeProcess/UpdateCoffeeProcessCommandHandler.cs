using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

public class UpdateCoffeeProcessCommandHandler
	: IRequestHandler<UpdateCoffeeProcessCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateCoffeeProcessCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateCoffeeProcessCommand request, CancellationToken ct)
	{
		var process = await _context.CoffeeProcesses
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (process is null)
			return Error.NotFound("CoffeeProcess.NotFound", "فرآیند یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.CoffeeProcesses.AnyAsync(p => p.Slug == slug && p.Id != process.Id && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		process.Name = request.Name.Trim();
		process.Slug = slug;
		process.Description = request.Description?.Trim();
		process.ImageUrl = request.ImageUrl?.Trim();
		process.IconUrl = request.IconUrl?.Trim();
		process.IsActive = request.IsActive;
		process.DisplayOrder = request.DisplayOrder;

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
