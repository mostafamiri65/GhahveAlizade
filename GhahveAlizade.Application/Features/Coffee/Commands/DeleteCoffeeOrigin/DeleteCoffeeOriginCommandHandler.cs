using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Coffee.Commands.DeleteCoffeeOrigin;

public class DeleteCoffeeOriginCommandHandler
	: IRequestHandler<DeleteCoffeeOriginCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteCoffeeOriginCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(
		DeleteCoffeeOriginCommand request,
		CancellationToken cancellationToken)
	{
		var origin = await _context.CoffeeOrigins
			.FirstOrDefaultAsync(o => o.Id == request.Id && !o.IsDeleted, cancellationToken);

		if (origin is null)
			return Error.NotFound("CoffeeOrigin.NotFound", "خاستگاه یافت نشد.");

		var isUsed = await _context.Products
			.AnyAsync(p => p.CoffeeOriginId == origin.Id && !p.IsDeleted, cancellationToken);

		if (isUsed)
			return Error.Conflict(
				"CoffeeOrigin.InUse",
				"این خاستگاه در محصولات استفاده شده و قابل حذف نیست.");

		_context.CoffeeOrigins.Remove(origin);
		await _context.SaveChangesAsync(cancellationToken);
		return Result.Success;
	}
}