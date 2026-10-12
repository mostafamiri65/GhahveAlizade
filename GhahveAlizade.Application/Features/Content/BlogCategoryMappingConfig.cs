using GhahveAlizade.Application.Features.Content.Dtos;
using GhahveAlizade.Domain.Content;
using Mapster;

namespace GhahveAlizade.Application.Features.Content;

public class BlogCategoryMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<BlogCategory, BlogCategoryDto>()
			.Map(dest => dest.ParentName, src => src.Parent != null ? src.Parent.Name : null)
			.Map(dest => dest.PostCount, src => src.Posts.Count(p => !p.IsDeleted))
			.Map(dest => dest.ChildrenCount, src => src.Children.Count(c => !c.IsDeleted));

		config.NewConfig<BlogCategory, BlogCategoryListItemDto>()
			.Map(dest => dest.ParentName, src => src.Parent != null ? src.Parent.Name : null)
			.Map(dest => dest.PostCount, src => src.Posts.Count(p => !p.IsDeleted));
	}
}