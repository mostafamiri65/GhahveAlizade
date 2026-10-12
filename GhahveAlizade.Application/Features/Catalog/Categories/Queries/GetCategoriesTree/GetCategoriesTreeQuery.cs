using ErrorOr;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesTree;

public record GetCategoriesTreeQuery(bool OnlyActive = true, long? ExcludeId = null)
	: IRequest<ErrorOr<List<CategoryTreeItemDto>>>;

public class CategoryTreeItemDto
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public long? ParentId { get; set; }
	public int Depth { get; set; }
	public string DisplayName { get; set; } = string.Empty;
}