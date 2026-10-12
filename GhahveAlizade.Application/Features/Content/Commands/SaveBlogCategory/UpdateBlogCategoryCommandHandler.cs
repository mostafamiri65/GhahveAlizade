using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public class UpdateBlogCategoryCommandHandler
	: IRequestHandler<UpdateBlogCategoryCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public UpdateBlogCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(UpdateBlogCategoryCommand request, CancellationToken ct)
	{
		var category = await _context.BlogCategories
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (category is null) return Error.NotFound("BlogCategory.NotFound", "دسته‌بندی یافت نشد.");

		if (request.ParentId == category.Id)
			return Error.Validation("BlogCategory.SelfParent", "دسته نمی‌تواند والد خودش باشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogCategories.AnyAsync(
			c => c.Slug == slug && c.Id != category.Id && !c.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		category.Name = request.Name.Trim();
		category.Slug = slug;
		category.Description = request.Description?.Trim();
		category.ImageUrl = request.ImageUrl?.Trim();
		category.ParentId = request.ParentId;
		category.DisplayOrder = request.DisplayOrder;
		category.IsActive = request.IsActive;
		category.MetaTitle = request.MetaTitle?.Trim();
		category.MetaDescription = request.MetaDescription?.Trim();
		category.CanonicalUrl = request.CanonicalUrl?.Trim();

		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
