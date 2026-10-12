using ErrorOr;
using GhahveAlizade.Application.Common;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Domain.Content;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public class CreateBlogCategoryCommandHandler
	: IRequestHandler<CreateBlogCategoryCommand, ErrorOr<long>>
{
	private readonly IAppDbContext _context;
	public CreateBlogCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<long>> Handle(CreateBlogCategoryCommand request, CancellationToken ct)
	{
		var baseSlug = SlugHelper.GenerateSlug(request.Slug ?? request.Name);
		var slug = baseSlug;
		var counter = 1;
		while (await _context.BlogCategories.AnyAsync(c => c.Slug == slug && !c.IsDeleted, ct))
			slug = $"{baseSlug}-{counter++}";

		var category = new BlogCategory
		{
			Name = request.Name.Trim(),
			Slug = slug,
			Description = request.Description?.Trim(),
			ImageUrl = request.ImageUrl?.Trim(),
			ParentId = request.ParentId,
			DisplayOrder = request.DisplayOrder,
			IsActive = request.IsActive,
			MetaTitle = request.MetaTitle?.Trim(),
			MetaDescription = request.MetaDescription?.Trim(),
			CanonicalUrl = request.CanonicalUrl?.Trim()
		};

		_context.BlogCategories.Add(category);
		await _context.SaveChangesAsync(ct);
		return category.Id;
	}
}
