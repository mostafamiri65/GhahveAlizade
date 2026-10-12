using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Seo.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoLandingPagesList;

public record GetSeoLandingPagesListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	bool? IsPublished = null, string? SortBy = "createdAt", string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<SeoLandingPageListItemDto>>>;