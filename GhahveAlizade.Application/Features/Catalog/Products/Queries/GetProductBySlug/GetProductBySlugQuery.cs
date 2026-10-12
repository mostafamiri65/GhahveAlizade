using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductBySlug;

public record GetProductBySlugQuery(string Slug) : IRequest<ErrorOr<ProductDetailDto>>;