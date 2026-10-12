using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Commands.SaveBlogCategory;

public class ToggleBlogCategoryActiveCommandHandler
	: IRequestHandler<ToggleBlogCategoryActiveCommand, ErrorOr<bool>>
{
	private readonly IAppDbContext _context;
	public ToggleBlogCategoryActiveCommandHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<bool>> Handle(ToggleBlogCategoryActiveCommand request, CancellationToken ct)
	{
		var c = await _context.BlogCategories
			.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, ct);
		if (c is null) return Error.NotFound("BlogCategory.NotFound", "دسته‌بندی یافت نشد.");

		c.IsActive = !c.IsActive;
		await _context.SaveChangesAsync(ct);
		return c.IsActive;
	}
}