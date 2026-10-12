using ErrorOr;
using GhahveAlizade.Domain.Enums;
using MediatR;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Commands.CreateAttribute;

public record CreateAttributeCommand(
    string Name,
    string? Slug,
    AttributeType Type,
    string? Unit,
    bool IsFilterable,
    bool IsRequired,
    bool IsVariant,
    int DisplayOrder,
    List<AttributeValueInput> Values)
    : IRequest<ErrorOr<long>>;

public record AttributeValueInput(
    long? Id,
    string Value,
    string? DisplayValue,
    string? ColorCode,
    int DisplayOrder);