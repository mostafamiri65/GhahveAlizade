using ErrorOr;
using GhahveAlizade.Application.Common.Models;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductsList;

public record GetProductsListQuery(
	int PageNumber = 1,
	int PageSize = 20,
	string? Search = null,
	long? CategoryId = null,
	long? BrandId = null,
	long? CoffeeOriginId = null,
	long? RoastProfileId = null,
	long? CoffeeProcessId = null,
	bool? IsActive = null,
	bool? IsPublished = null,
	bool? IsFeatured = null,
	bool? HasVariants = null,
	decimal? MinPrice = null,
	decimal? MaxPrice = null,
	bool LowStockOnly = false,
	string? SortBy = "createdAt",
	string? SortDir = "desc")
	: IRequest<ErrorOr<PaginatedList<ProductListItemDto>>>;