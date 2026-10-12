using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.ToggleProductActive;

public class ToggleProductActiveCommandHandler
	: IRequestHandler<ToggleProductActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleProductActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(
		ToggleProductActiveCommand request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		product.IsActive = !product.IsActive;
		await _context.SaveChangesAsync(cancellationToken);

		return product.IsActive;
	}
}