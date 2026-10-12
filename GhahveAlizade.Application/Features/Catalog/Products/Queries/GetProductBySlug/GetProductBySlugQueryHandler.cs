using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductBySlug;

/// <summary>
/// نسخه عمومی GetProductById — فقط محصولات منتشرشده و فعال را برمی‌گرداند.
/// </summary>
public class GetProductBySlugQueryHandler
	: IRequestHandler<GetProductBySlugQuery, ErrorOr<ProductDetailDto>>
{
	private readonly IAppDbContext _context;

	public GetProductBySlugQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<ProductDetailDto>> Handle(
		GetProductBySlugQuery request,
		CancellationToken cancellationToken)
	{
		var product = await _context.Products
			.AsNoTracking()
			.Include(p => p.Category)
			.Include(p => p.Brand)
			.Include(p => p.CoffeeOrigin)
			.Include(p => p.CoffeeProcess)
			.Include(p => p.RoastProfile)
			.Include(p => p.Variants.Where(v => !v.IsDeleted && v.IsActive).OrderBy(v => v.DisplayOrder))
			.Include(p => p.Images.OrderBy(i => i.DisplayOrder))
			.Include(p => p.Attributes).ThenInclude(a => a.Attribute)
			.Include(p => p.Attributes).ThenInclude(a => a.AttributeValue)
			.Include(p => p.FlavorNotes)
			.Where(p => p.Slug == request.Slug
				&& !p.IsDeleted
				&& p.IsActive
				&& p.IsPublished)
			.FirstOrDefaultAsync(cancellationToken);

		if (product is null)
			return Error.NotFound("Product.NotFound", "محصول یافت نشد یا دیگر در دسترس نیست.");

		return product.Adapt<ProductDetailDto>();
	}
}