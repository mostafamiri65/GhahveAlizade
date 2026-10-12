using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Coffee;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

public class CreateCoffeeProcessCommandHandler
	: IRequestHandler<CreateCoffeeProcessCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateCoffeeProcessCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateCoffeeProcessCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.CoffeeProcesses.AnyAsync(p => p.Slug == slug && !p.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var process = new CoffeeProcess
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim(),
			ImageUrl = request.ImageUrl?.Trim(),
			IconUrl = request.IconUrl?.Trim(),
			IsActive = request.IsActive,
			DisplayOrder = request.DisplayOrder
		};

		_context.CoffeeProcesses.Add(process);
		await _context.SaveChangesAsync(ct);
		return process.Id;
	}
}
