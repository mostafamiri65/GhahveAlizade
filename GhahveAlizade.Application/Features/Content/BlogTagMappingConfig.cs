using GhahveAlizade.Application.Features.Content.Dtos;
using GhahveAlizade.Domain.Content;
using Mapster;

namespace GhahveAlizade.Application.Features.Content;

public class BlogTagMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<BlogTag, BlogTagDto>()
			.Map(dest => dest.PostCount, src => src.PostTags.Count(pt => !pt.IsDeleted));

		config.NewConfig<BlogTag, BlogTagListItemDto>()
			.Map(dest => dest.PostCount, src => src.PostTags.Count(pt => !pt.IsDeleted));
	}
}