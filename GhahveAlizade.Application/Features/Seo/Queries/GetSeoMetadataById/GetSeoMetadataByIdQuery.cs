using ErrorOr;
using GhahveAlizade.Application.Features.Seo.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Seo.Queries.GetSeoMetadataById;

public record GetSeoMetadataByIdQuery(long Id) : IRequest<ErrorOr<SeoMetadataDto>>;
