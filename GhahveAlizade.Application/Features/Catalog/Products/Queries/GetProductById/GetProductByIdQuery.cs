using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductById;

public record GetProductByIdQuery(long Id) : IRequest<ErrorOr<ProductDetailDto>>;