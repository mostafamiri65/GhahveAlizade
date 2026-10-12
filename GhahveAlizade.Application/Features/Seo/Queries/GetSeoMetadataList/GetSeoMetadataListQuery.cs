using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Seo.Dtos;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataList;

public record GetSeoMetadataListQuery(
	int PageNumber = 1, int PageSize = 20, string? Search = null,
	SeoEntityType? EntityType = null)
	: IRequest<ErrorOr<PaginatedList<SeoMetadataListItemDto>>>;
