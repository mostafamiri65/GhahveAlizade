using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler
	: IRequestHandler<DeleteCategoryCommand, ErrorOr<Success>>
{
	private readonly IAppDbContext _context;

	public DeleteCategoryCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<Success>> Handle(
		DeleteCategoryCommand request,
		CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

		if (category is null)
			return Error.NotFound("Category.NotFound", "دسته‌بندی یافت نشد.");

		// چک: آیا محصولی داره؟
		var hasProducts = await _context.Products
			.AnyAsync(p => p.CategoryId == category.Id && !p.IsDeleted, cancellationToken);

		if (hasProducts)
			return Error.Conflict(
				"Category.HasProducts",
				"این دسته‌بندی دارای محصول است و قابل حذف نیست. ابتدا محصولات را منتقل کنید.");

		// چک: آیا زیردسته داره؟
		var hasChildren = await _context.Categories
			.AnyAsync(c => c.ParentId == category.Id && !c.IsDeleted, cancellationToken);

		if (hasChildren)
			return Error.Conflict(
				"Category.HasChildren",
				"این دسته‌بندی دارای زیردسته است و قابل حذف نیست.");

		// Soft Delete
		_context.Categories.Remove(category);
		await _context.SaveChangesAsync(cancellationToken);

		return Result.Success;
	}
}