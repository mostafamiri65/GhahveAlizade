using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler
	: IRequestHandler<UpdateCategoryCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public UpdateCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(
		UpdateCategoryCommand request,
		CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

		if (category is null)
			return Error.NotFound("Category.NotFound", "دسته‌بندی یافت نشد.");

		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = await EnsureUniqueSlugAsync(baseSlug, category.Id, cancellationToken);

		category.Name = request.Name.Trim();
		category.Slug = slug;
		category.Description = request.Description?.Trim();
		category.ImageUrl = request.ImageUrl?.Trim();
		category.IconUrl = request.IconUrl?.Trim();
		category.ParentId = request.ParentId;
		category.DisplayOrder = request.DisplayOrder;
		category.IsActive = request.IsActive;
		category.IsFeatured = request.IsFeatured;
		category.MetaTitle = request.MetaTitle?.Trim();
		category.MetaDescription = request.MetaDescription?.Trim();
		category.MetaKeywords = request.MetaKeywords?.Trim();

		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success;
	}

	private async Task<string> EnsureUniqueSlugAsync(
		string baseSlug, long excludeId, CancellationToken ct)
	{
		var slug = baseSlug;
		var counter = 1;

		while (await _context.Categories.AnyAsync(
			c => c.Slug == slug && c.Id != excludeId && !c.IsDeleted, ct))
		{
			slug = $"{baseSlug}-{counter}";
			counter++;
		}

		return slug;
	}
}