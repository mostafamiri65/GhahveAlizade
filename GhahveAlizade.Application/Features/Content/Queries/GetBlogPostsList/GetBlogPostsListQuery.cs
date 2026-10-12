using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Content.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Content.Queries.GetBlogPostsList;

public record GetBlogPostsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? CategoryId = null,
	bool? IsPublished = null,
	bool? IsFeatured = null,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<BlogPostListItemDto>>>;