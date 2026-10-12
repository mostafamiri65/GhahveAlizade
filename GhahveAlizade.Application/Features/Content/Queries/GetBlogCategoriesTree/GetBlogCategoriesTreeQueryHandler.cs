using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesTree;

public class GetBlogCategoriesTreeQueryHandler
	: IRequestHandler<GetBlogCategoriesTreeQuery, ErrorOr<List<BlogCategoryTreeItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetBlogCategoriesTreeQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<BlogCategoryTreeItemDto>>> Handle(
		GetBlogCategoriesTreeQuery request, CancellationToken ct)
	{
		var query = _context.BlogCategories.AsNoTracking().Where(c => !c.IsDeleted);
		if (request.OnlyActive) query = query.Where(c => c.IsActive);

		var all = await query
			.Select(c => new { c.Id, c.Name, c.ParentId, c.DisplayOrder })
			.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name)
			.ToListAsync(ct);

		var result = new List<BlogCategoryTreeItemDto>();
		var excluded = new HashSet<long>();
		if (request.ExcludeId.HasValue) excluded.Add(request.ExcludeId.Value);

		void Traverse(long? parentId, int depth)
		{
			foreach (var item in all.Where(c => c.ParentId == parentId && !excluded.Contains(c.Id)))
			{
				var prefix = string.Concat(Enumerable.Repeat("— ", depth));
				result.Add(new BlogCategoryTreeItemDto
				{
					Id = item.Id,
					Name = item.Name,
					ParentId = item.ParentId,
					Depth = depth,
					DisplayName = $"{prefix}{item.Name}"
				});
				Traverse(item.Id, depth + 1);
			}
		}

		Traverse(null, 0);
		return result;
	}
}