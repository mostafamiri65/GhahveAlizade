using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler
	: IRequestHandler<DeleteProductCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteProductCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(
		DeleteProductCommand request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		// چک: آیا در سفارشات استفاده شده؟
		var hasOrders = await _context.OrderItems
			.AnyAsync(oi => oi.ProductId == product.Id && !oi.IsDeleted, cancellationToken);

		if (hasOrders)
			return Error.Conflict(
				"Product.HasOrders",
				"این محصول در سفارشات استفاده شده و قابل حذف نیست. می‌توانید آن را غیرفعال کنید.");

		// Soft Delete
		_context.Products.Remove(product);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success;
	}
}