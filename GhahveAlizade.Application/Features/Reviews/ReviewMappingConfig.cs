using GhahveAlizade.Application.Features.Reviews.Dtos;
using GhahveAlizade.Domain.Review;
using Mapster;

namespace GhahveAlizade.Application.Features.Reviews;

public class ReviewMappingConfig : IRegister
{
	public void Register(TypeAdapterConfig config)
	{
		config.NewConfig<ProductReview, ReviewListItemDto>()
			.Map(dest => dest.ProductName,
				src => src.Product != null ? src.Product.Name : string.Empty)
			.Map(dest => dest.ProductSlug,
				src => src.Product != null ? src.Product.Slug : null)
			.Map(dest => dest.CustomerName,
				src => src.Customer != null
					? (src.Customer.FirstName + " " + src.Customer.LastName).Trim()
					: null)
			.Map(dest => dest.ReplyCount, src => src.Replies.Count(r => !r.IsDeleted))
			.Map(dest => dest.ImageCount, src => src.Images.Count(i => !i.IsDeleted));

		config.NewConfig<ProductReview, ReviewDetailDto>()
			.Map(dest => dest.ProductName,
				src => src.Product != null ? src.Product.Name : string.Empty)
			.Map(dest => dest.ProductSlug,
				src => src.Product != null ? src.Product.Slug : null)
			.Map(dest => dest.CustomerName,
				src => src.Customer != null
					? (src.Customer.FirstName + " " + src.Customer.LastName).Trim()
					: null)
			.Map(dest => dest.ReplyCount, src => src.Replies.Count(r => !r.IsDeleted))
			.Map(dest => dest.ImageCount, src => src.Images.Count(i => !i.IsDeleted))
			.Map(dest => dest.Images, src => src.Images.Where(i => !i.IsDeleted).ToList())
			.Map(dest => dest.Replies, src => src.Replies.Where(r => !r.IsDeleted).OrderBy(r => r.CreatedAtUtc).ToList());

		config.NewConfig<ReviewImage, ReviewImageDto>();
		config.NewConfig<ReviewReply, ReviewReplyDto>();
	}
}