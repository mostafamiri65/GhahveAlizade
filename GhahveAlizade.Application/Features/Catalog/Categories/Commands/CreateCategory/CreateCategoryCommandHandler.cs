using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
	: IRequestHandler<CreateCategoryCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;

	public CreateCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(
		CreateCategoryCommand request,
		CancellationToken cancellationToken)
	{
		// ساخت Slug یکتا
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = await EnsureUniqueSlugAsync(baseSlug, null, cancellationToken);

		var category = new Category
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim(),
			ImageUrl = request.ImageUrl?.Trim(),
			IconUrl = request.IconUrl?.Trim(),
			ParentId = request.ParentId,
			DisplayOrder = request.DisplayOrder,
			IsActive = request.IsActive,
			IsFeatured = request.IsFeatured,
			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			MetaKeywords = request.MetaKeywords?.Trim()
		};

		_context.Categories.Add(category);
		await _context.SaveChangesAsync(cancellationToken);

		return category.Id;
	}

	private async Task<string> EnsureUniqueSlugAsync(
		string baseSlug, long? excludeId, CancellationToken ct)
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