using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Products.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Products.Queries.GetProductFormData;

public record GetProductFormDataQuery : IRequest<ErrorOr<ProductFormDataDto>>;