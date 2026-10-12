using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public class DeleteBlogCategoryCommandHandler
	: IRequestHandler<DeleteBlogCategoryCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;
	public DeleteBlogCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(DeleteBlogCategoryCommand request, CancellationToken ct)
	{
		var category = await _context.BlogCategories
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, ct);

		if (category is null) return Error.NotFound("BlogCategory.NotFound", "دسته‌بندی یافت نشد.");

		var hasPosts = await _context.BlogPosts
			.AnyAsync(p => p.CategoryId == category.Id && !p.IsDeleted, ct);
		if (hasPosts)
			return Error.Conflict("BlogCategory.HasPosts", "این دسته دارای مقاله است و قابل حذف نیست.");

		var hasChildren = await _context.BlogCategories
			.AnyAsync(c => c.ParentId == category.Id && !c.IsDeleted, ct);
		if (hasChildren)
			return Error.Conflict("BlogCategory.HasChildren", "این دسته دارای زیردسته است.");

		_context.BlogCategories.Remove(category);
		await _context.SaveChangesAsync(ct);
		return Result.Success;
	}
}
