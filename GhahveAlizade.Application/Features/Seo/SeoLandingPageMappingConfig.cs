using GhahveAlizade.Application.Features.Seo.Dtos;
using GhahveAlizade.Domain.SEO;
using Mapster;

namespace GhahveAlizade.Application.Features.Seo;

public class SeoLandingPageMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<SeoLandingPage, SeoLandingPageDto>();
		config.NewConfig<SeoLandingPage, SeoLandingPageListItemDto>();
	}
}