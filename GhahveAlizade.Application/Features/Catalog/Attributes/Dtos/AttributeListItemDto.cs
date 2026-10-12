using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;

public class AttributeListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public AttributeType Type { get; set; }
    public string? TypeName { get; set; }
    public string? Unit { get; set; }
    public bool IsFilterable { get; set; }
    public bool IsRequired { get; set; }
    public bool IsVariant { get; set; }
    public int DisplayOrder { get; set; }
    public int ValueCount { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}