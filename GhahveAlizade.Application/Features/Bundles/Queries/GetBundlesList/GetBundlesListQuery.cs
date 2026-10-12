using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Bundles.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundlesList;

public record GetBundlesListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	bool? IsActive = null, bool? IsFeatured = null,
	string? SortBy = "createdAt", string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<ProductBundleListItemDto>>>;
