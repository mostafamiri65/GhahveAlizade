using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Catalog.Categories.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler
	: IRequestHandler<GetCategoryByIdQuery, ErrorOr<CategoryDto>>
{
	private readonly IAppDbContext _context;

	public GetCategoryByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<CategoryDto>> Handle(
		GetCategoryByIdQuery request,
		CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.AsNoTracking()
			.Where(c => c.Id == request.Id && !c.IsDeleted)
			.Select(c => new CategoryDto
			{
				Id = c.Id,
				Name = c.Name,
				Slug = c.Slug,
				Description = c.Description,
				ImageUrl = c.ImageUrl,
				IconUrl = c.IconUrl,
				ParentId = c.ParentId,
				ParentName = c.Parent != null ? c.Parent.Name : null,
				DisplayOrder = c.DisplayOrder,
				IsActive = c.IsActive,
				IsFeatured = c.IsFeatured,
				MetaTitle = c.MetaTitle,
				MetaDescription = c.MetaDescription,
				MetaKeywords = c.MetaKeywords,
				CanonicalUrl = c.CanonicalUrl,
				OgImage = c.OgImage,
				CreatedAtUtc = c.CreatedAtUtc,
				UpdatedAtUtc = c.UpdatedAtUtc,
				ChildrenCount = c.Children.Count(ch => !ch.IsDeleted),
				ProductCount = c.Products.Count(p => !p.IsDeleted)
			})
			.FirstOrDefaultAsync(cancellationToken);

		if (category is null)
			return Error.NotFound("Category.NotFound", "دسته‌بندی یافت نشد.");

		return category;
	}
}