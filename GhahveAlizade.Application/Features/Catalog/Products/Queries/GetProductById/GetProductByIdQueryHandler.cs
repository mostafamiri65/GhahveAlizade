using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler
	: IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetProductByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ProductDetailDto>> Handle(
		GetProductByIdQuery request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.AsNoTracking()
			.Include(p => p.Category)
			.Include(p => p.Brand)
			.Include(p => p.CoffeeOrigin)
			.Include(p => p.CoffeeProcess)
			.Include(p => p.RoastProfile)
			.Include(p => p.Variants)
			.Include(p => p.Images)
			.Include(p => p.Attributes).ThenInclude(a => a.Attribute)
			.Include(p => p.Attributes).ThenInclude(a => a.AttributeValue)
			.Include(p => p.FlavorNotes)
			.Where(p => p.Id == request.Id && !p.IsDeleted)
			.FirstOrDefaultAsync(cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد.");

		return product.Adapt<ProductDetailDto>();
	}
}