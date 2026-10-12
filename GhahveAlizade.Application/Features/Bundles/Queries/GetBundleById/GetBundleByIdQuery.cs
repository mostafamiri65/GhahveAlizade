using ErrorOr;
using GhahveAlizade.Application.Features.Bundles.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Bundles.Queries.GetBundleById;

public record GetBundleByIdQuery(long Id) : IRequest<ErrorOr<ProductBundleDto>>;
