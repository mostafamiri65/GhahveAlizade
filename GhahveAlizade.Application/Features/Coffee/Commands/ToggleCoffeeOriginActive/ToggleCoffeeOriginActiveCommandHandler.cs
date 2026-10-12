using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.ToggleCoffeeOriginActive;

public class ToggleCoffeeOriginActiveCommandHandler
	: IRequestHandler<ToggleCoffeeOriginActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleCoffeeOriginActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(
		ToggleCoffeeOriginActiveCommand request,
		CancellationToken cancellationToken)
	{
		var origin = await _context.CoffeeOrigins
			.FirstOrDefaultAsync(o => o.Id == request.Id && !o.IsDeleted, cancellationToken);

		if (origin is null)
			return Error.NotFound("CoffeeOrigin.NotFound", "خاستگاه یافت نشد.");

		origin.IsActive = !origin.IsActive;
		await _context.SaveChangesAsync(cancellationToken);
		return origin.IsActive;
	}
}