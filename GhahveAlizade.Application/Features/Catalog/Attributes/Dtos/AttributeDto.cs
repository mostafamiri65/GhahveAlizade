using GhahveAlizade.Domain.Enums;

namespace GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;

public class AttributeDto
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

    public List<AttributeValueDto> Values { get; set; } = new();
    public int ProductCount { get; set; }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}

public class AttributeValueDto
{
    public long Id { get; set; }
    public string Value { get; set; } = string.Empty;
    public string? DisplayValue { get; set; }
    public string? ColorCode { get; set; }
    public int DisplayOrder { get; set; }
}