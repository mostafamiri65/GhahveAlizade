using GhahveAlizade.Application.Features.Catalog.Attributes.Dtos;
using GhahveAlizade.Domain.Enums;
using Mapster;

namespace GhahveAlizade.Application.Features.Catalog.Attributes;

public class AttributeMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Catalog.Attribute, AttributeDto>()
            .Map(dest => dest.TypeName, src => GetTypeName(src.Type))
            .Map(dest => dest.ProductCount, src => src.ProductAttributes.Count(pa => !pa.IsDeleted))
            .Map(dest => dest.Values, src => src.Values
                .Where(v => !v.IsDeleted)
                .OrderBy(v => v.DisplayOrder)
                .ToList());

        config.NewConfig<Domain.Catalog.Attribute, AttributeListItemDto>()
            .Map(dest => dest.TypeName, src => GetTypeName(src.Type))
            .Map(dest => dest.ValueCount, src => src.Values.Count(v => !v.IsDeleted));

        config.NewConfig<Domain.Catalog.AttributeValue, AttributeValueDto>();
    }

    private static string GetTypeName(AttributeType type) => type switch
    {
        AttributeType.Text => "متن",
        AttributeType.Number => "عدد",
        AttributeType.Bool => "بله/خیر",
        AttributeType.Select => "انتخابی",
        AttributeType.MultiSelect => "چندانتخابی",
        AttributeType.Color => "رنگ",
        AttributeType.Date => "تاریخ",
        _ => "نامشخص"
    };
}