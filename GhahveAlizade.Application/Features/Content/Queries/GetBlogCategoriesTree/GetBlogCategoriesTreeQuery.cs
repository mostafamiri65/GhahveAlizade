using ErrorOr;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesTree;

public record GetBlogCategoriesTreeQuery(bool OnlyActive = true, long? ExcludeId = null)
	: IRequest<ErrorOr<List<BlogCategoryTreeItemDto>>>;
