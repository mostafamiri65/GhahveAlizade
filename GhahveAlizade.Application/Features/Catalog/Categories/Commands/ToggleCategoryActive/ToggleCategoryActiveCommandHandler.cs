using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Commands.ToggleCategoryActive;

public class ToggleCategoryActiveCommandHandler
	: IRequestHandler<ToggleCategoryActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;

	public ToggleCategoryActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(
		ToggleCategoryActiveCommand request,
		CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.FirstOrDefaultAsync(c => c.Id == request.Id && !c.IsDeleted, cancellationToken);

		if (category is null)
			return Error.NotFound("Category.NotFound", "دسته‌بندی یافت نشد.");

		category.IsActive = !category.IsActive;
		await _context.SaveChangesAsync(cancellationToken);

		return category.IsActive;
	}
}