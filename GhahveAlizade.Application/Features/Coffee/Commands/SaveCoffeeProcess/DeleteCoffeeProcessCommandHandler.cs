using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.SaveCoffeeProcess;

public class DeleteCoffeeProcessCommandHandler
	: IRequestHandler<DeleteCoffeeProcessCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteCoffeeProcessCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteCoffeeProcessCommand request, CancellationToken ct)
	{
		var process = await _context.CoffeeProcesses
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, ct);

		if (process is null)
			return Error.NotFound("CoffeeProcess.NotFound", "فرآیند یافت نشد.");

		var isUsed = await _context.Products
			.AnyAsync(p => p.CoffeeProcessId == process.Id && !p.IsDeleted, ct);

		if (isUsed)
			return Error.Conflict("CoffeeProcess.InUse", "این فرآیند در محصولات استفاده شده است.");

		_context.CoffeeProcesses.Remove(process);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
