using GhahveAlizade.Application.Features.Seo.Dtos;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.SEO;
using Mapster;

namespace GhahveAlizade.Application.Features.Seo;

public class SeoMetadataMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<SeoMetadata, SeoMetadataDto>()
			.Map(dest => dest.EntityTypeName, src => GetEntityTypeName(src.EntityType))
			.Map(dest => dest.RobotsName, src => src.Robots.ToString());

		config.NewConfig<SeoMetadata, SeoMetadataListItemDto>()
			.Map(dest => dest.EntityTypeName, src => GetEntityTypeName(src.EntityType));
	}

	private static string GetEntityTypeName(SeoEntityType t) => t switch
	{
		SeoEntityType.HomePage => "صفحه اصلی",
		SeoEntityType.Product => "محصول",
		SeoEntityType.Category => "دسته‌بندی",
		SeoEntityType.Brand => "برند",
		SeoEntityType.BlogPost => "مقاله",
		SeoEntityType.BlogCategory => "دسته وبلاگ",
		SeoEntityType.BlogTag => "برچسب وبلاگ",
		SeoEntityType.LandingPage => "صفحه فرود",
		SeoEntityType.StaticPage => "صفحه ثابت",
		SeoEntityType.ProductBundle => "بسته محصول",
		_ => t.ToString()
	};
}