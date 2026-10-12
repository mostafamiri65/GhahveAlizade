using ErrorOr;
using GhahveAlizade.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesTree;

public class GetCategoriesTreeQueryHandler
	: IRequestHandler<GetCategoriesTreeQuery, ErrorOr<List<CategoryTreeItemDto>>>
{
	private readonly IAppDbContext _context;

	public GetCategoriesTreeQueryHandler(IAppDbContext context) => _context = context;

	public async Task<ErrorOr<List<CategoryTreeItemDto>>> Handle(
		GetCategoriesTreeQuery request,
		CancellationToken cancellationToken)
	{
		var query = _context.Categories
			.AsNoTracking()
			.Where(c => !c.IsDeleted);

		if (request.OnlyActive)
			query = query.Where(c => c.IsActive);

		// برای جلوگیری از چرخه در ویرایش: خود دسته و فرزندانش رو حذف کن
		var excludedIds = new HashSet<long>();
		if (request.ExcludeId.HasValue)
		{
			excludedIds.Add(request.ExcludeId.Value);
		}

		var all = await query
			.Select(c => new { c.Id, c.Name, c.ParentId, c.DisplayOrder })
			.OrderBy(c => c.DisplayOrder)
			.ThenBy(c => c.Name)
			.ToListAsync(cancellationToken);

		var flat = new List<CategoryTreeItemDto>();

		void Traverse(long? parentId, int depth)
		{
			var children = all
				.Where(c => c.ParentId == parentId && !excludedIds.Contains(c.Id))
				.OrderBy(c => c.DisplayOrder)
				.ThenBy(c => c.Name);

			foreach (var item in children)
			{
				var prefix = string.Concat(Enumerable.Repeat("— ", depth));
				flat.Add(new CategoryTreeItemDto
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

		return flat;
	}
}