using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Brands.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Brands.Queries.GetBrandById;

public record GetBrandByIdQuery(long Id) : IRequest<ErrorOr<BrandDto>>;