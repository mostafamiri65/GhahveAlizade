using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogCategoriesList;

public record GetBlogCategoriesListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? ParentId = null,
	bool? IsActive = null,
	string? SortBy = "displayOrder",
	string? SortDir = "asc")
	: IRequest<ErrorOr<PaginatedList<BlogCategoryListItemDto>>>;