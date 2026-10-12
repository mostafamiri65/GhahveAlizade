using ErrorOr;
using GhahveAlizade.Application.Features.Catalog.Attributes.Commands.CreateAttribute;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.UpdateAttribute;

public record UpdateAttributeCommand(
    long Id,
    string Name,
    string? Slug,
    AttributeType Type,
    string? Unit,
    bool IsFilterable,
    bool IsRequired,
    bool IsVariant,
    int DisplayOrder,
    List<AttributeValueInput> Values)
    : IRequest<ErrorOr<Success>>;