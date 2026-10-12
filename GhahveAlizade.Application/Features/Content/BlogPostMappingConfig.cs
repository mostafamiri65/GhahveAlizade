using GhahveAlizade.Application.Features.Content.Dtos;
using GhahveAlizade.Domain.Content;
using Mapster;

namespace GhahveAlizade.Application.Features.Content;

public class BlogPostMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<BlogPost, BlogPostDto>()
			.Map(dest => dest.CategoryName,
				src => src.Category != null ? src.Category.Name : null)
			.Map(dest => dest.TagIds,
				src => src.PostTags.Where(pt => !pt.IsDeleted).Select(pt => pt.BlogTagId).ToList())
			.Map(dest => dest.TagNames,
				src => src.PostTags.Where(pt => !pt.IsDeleted)
					.Select(pt => pt.BlogTag.Name).ToList());

		config.NewConfig<BlogPost, BlogPostListItemDto>()
			.Map(dest => dest.CategoryName,
				src => src.Category != null ? src.Category.Name : null)
			.Map(dest => dest.TagCount,
				src => src.PostTags.Count(pt => !pt.IsDeleted));
	}
}