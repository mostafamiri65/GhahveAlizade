using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoryById;

public class GetBlogCategoryByIdQueryHandler
	: IRequestHandler<GetBlogCategoryByIdQuery, ErrorOr<BlogCategoryDto>>
{
	private readonly IAppDbContext _context;
	public GetBlogCategoryByIdQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<BlogCategoryDto>> Handle(GetBlogCategoryByIdQuery request, CancellationToken ct)
	{
		var category = await _context.BlogCategories
			.AsNoTracking()
			.Include(c => c.Parent)
			.Include(c => c.Posts)
			.Include(c => c.Children)
			.Where(c => c.Id == request.Id && !c.IsDeleted)
			.FirstOrDefaultAsync(ct);

		if (category is null) return Error.NotFound("BlogCategory.NotFound", "دسته‌بندی یافت نشد.");
		return category.Adapt<BlogCategoryDto>();
	}
}