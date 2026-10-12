using GhahveAlizade.Application.Features.Seo.Dtos;
using GhahveAlizade.Domain.Enums;
using GhahveAlizade.Domain.SEO;
using Mapster;

namespace GhahveAlizade.Application.Features.Seo;

public class RedirectMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<Redirect, RedirectDto>()
			.Map(dest => dest.TypeName, src => GetTypeName(src.Type));

		config.NewConfig<Redirect, RedirectListItemDto>()
			.Map(dest => dest.TypeName, src => GetTypeName(src.Type));
	}

	private static string GetTypeName(RedirectType t) => t switch
	{
		RedirectType.Permanent301 => "301 دائمی",
		RedirectType.Temporary302 => "302 موقت",
		RedirectType.Permanent308 => "308 دائمی",
		_ => t.ToString()
	};
}