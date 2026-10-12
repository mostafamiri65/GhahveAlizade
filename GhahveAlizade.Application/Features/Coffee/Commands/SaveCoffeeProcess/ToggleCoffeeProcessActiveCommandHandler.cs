using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

public class ToggleCoffeeProcessActiveCommandHandler
	: IRequestHandler<ToggleCoffeeProcessActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleCoffeeProcessActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleCoffeeProcessActiveCommand request, CancellationToken ct)
	{
		var process = await _context.CoffeeProcesses
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (process is null)
			return Error.NotFound("CoffeeProcess.NotFound", "فرآیند یافت نشد.");

		process.IsActive = !process.IsActive;
		await _context.SaveChangesAsync(ct);
		return process.IsActive;
	}
}