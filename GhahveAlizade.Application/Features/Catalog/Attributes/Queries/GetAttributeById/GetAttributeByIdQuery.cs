using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Queries.GetAttributeById;

public record GetAttributeByIdQuery(long Id) : IRequest<ErrorOr<AttributeDto>>;