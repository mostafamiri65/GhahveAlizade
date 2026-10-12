using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Categories.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Categories.Queries.GetCategoriesList;

public record GetCategoriesListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? ParentId = null,
	bool? IsActive = null,
	string? SortBy = "displayOrder",
	string? SortDir = "asc")
	: IRequest<ErrorOr<PaginatedList<CategoryListItemDto>>>;