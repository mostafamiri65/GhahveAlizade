using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Commands.PublishProduct;

public class PublishProductCommandHandler
	: IRequestHandler<PublishProductCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	private readonly IDateTimeProvider _dateTime;

	public PublishProductCommandHandler(
		IAppDbContext context,
		IDateTimeProvider dateTime)
	{
		_context = context;
		_dateTime = dateTime;
	}

	public async Task<ErrorOr<bool>> Handle(
		PublishProductCommand request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.FirstOrDefaultAsync(p => p.Id == request.Id && !p.IsDeleted, cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		product.IsPublished = !product.IsPublished;

		if (product.IsPublished && product.PublishedAtUtc is null)
			product.PublishedAtUtc = _dateTime.UtcNow;

		await _context.SaveChangesAsync(cancellationToken);

		return product.IsPublished;
	}
}